# ERS Fiscalisation API

Middleware between our invoicing systems (EMA and Macmobile) and the Eswatini Revenue Service (ERS) Sales Data Controller (SDC).

Each system saves its invoice first, then calls this API with the full invoice details. The API sends the invoice to the SDC, which signs it and reports it to ERS. The API returns the SDC invoice number, verification URL, QR code and receipt text. The calling system prints these on the invoice.

The API does NOT read SAP for these calls. It trusts the payload, after arithmetic checks.

## Flow

1. The caller saves the invoice in its own system and gets its unique number.
2. The caller calls `POST /api/fiscal/invoices` with its API key, the lines, VAT groups, cashier and payments.
3. The API maps VAT groups to SDC tax labels, sends the invoice to the SDC and stores the result in `FiscalInvoices`.
4. The caller prints the invoice with the SDC number and QR code. Do NOT print if the call did not return 200.

Macmobile later pushes its invoices to EMA for reporting. EMA must never call this API for those invoices.

## Authentication

Every endpoint requires an API key in the `X-Api-Key` request header.

```
X-Api-Key: <your key>
```

Each caller (EMA, each Macmobile user) gets its **own** key. Each key is locked to one source system:

| Key name (example) | SourceSystem | Can act on |
|--------------------|--------------|------------|
| EMA                | EMA          | EMA invoices only |
| Macmobile-A        | MACMOBILE    | MACMOBILE invoices only |
| Macmobile-B        | MACMOBILE    | MACMOBILE invoices only |
| Shared (dev only)  | `*`          | anything |

Why one key per caller: a leaked key can be removed without breaking the others, and each call can be traced to a caller. Never use a `*` key in production.

| Status | Meaning |
|--------|---------|
| 401 | Key missing or not recognised. |
| 403 | Key is valid but not allowed to act on this `sourceSystem` (e.g. the EMA key sending a MACMOBILE invoice). |

If no keys are configured, every request returns 401 (fail closed).

Keys are compared by SHA-256 hash with a constant-time comparison and are never logged.

## Source reference (idempotency key)

| SourceSystem | SourceReference           |
|--------------|---------------------------|
| EMA          | SAP DocEntry (not DocNum) |
| MACMOBILE    | Macmobile invoice number (U_MacmobileInv), unique across devices and across all Macmobile callers |

`SourceSystem + SourceReference` is unique. Calling again with the same key returns the stored result and never fiscalises twice, so retries after a lost response are safe.

## Endpoints

Base route: `/api/fiscal`

### POST /api/fiscal/invoices

Headers: `X-Api-Key`, `Content-Type: application/json`

Request:

```json
{
  "sourceSystem": "MACMOBILE",
  "sourceReference": "MM100234",
  "cashier": "jdlamini",
  "invoiceDate": "2026-10-14T09:22:00",
  "invoiceTotal": 230.00,
  "payments": [
    { "paymentType": "Cash", "amount": 230.00 }
  ],
  "lines": [
    {
      "itemCode": "UNIPCS69606823",
      "description": "Lifebuoy Toilet Soap",
      "unit": "Cases",
      "quantity": 2,
      "unitPrice": 100.00,
      "lineTotal": 200.00,
      "lineTotalInclVat": 230.00,
      "vatGroup": "S1"
    }
  ]
}
```

Rules:
- `unitPrice` is the price AFTER line discount, EXCLUDING VAT.
- `lineTotal` is excluding VAT. `lineTotalInclVat` is including VAT.
- `invoiceDate` must be a full date and time, not just a date.
- `paymentType` is one of: Cash, Card, Check, WireTransfer, Voucher, MobileMoney, Other.
- Payments must add up to the sum of `lineTotalInclVat`.
- `invoiceTotal` is optional. If sent, it must match the sum of the lines.
- `vatGroup` must exist in the `TaxLabels` config. Callers never send SDC tax labels.

Success (200):

```json
{
  "requestId": "MM-MM100234",
  "sourceSystem": "MACMOBILE",
  "sourceReference": "MM100234",
  "sdcInvoiceNumber": "35VXGCP6-35VXGCP6-16",
  "sdcDateTime": "2026-10-14T09:22:36+00:00",
  "invoiceCounter": "16/16NS",
  "verificationUrl": "https://...",
  "verificationQrCode": "R0lGODlh...",
  "journal": "============ FISCAL INVOICE ============ ...",
  "totalAmount": 230.00,
  "totalTax": 30.00
}
```

`verificationQrCode` is a base64 encoded GIF IMAGE, not text. Decode it to an image (or use `data:image/gif;base64,...`). Minimum print size is about 42 x 42 mm. The invoice must show the SDC invoice number.

| Status | Meaning |
|--------|---------|
| 200 | Signed. Safe to print. |
| 400 | Invalid payload (arithmetic, unknown VAT group, bad payment type). Fix and resend. |
| 401 | Missing or invalid API key. |
| 403 | API key not allowed for this source system. |
| 409 | Pending (SDC did not answer, outcome unknown) or already being processed. Do not print. Retry the same call later. |
| 502 | SDC rejected or unreachable. Do not print. The same call may be retried. |
| 500 | Unexpected error. |

### GET /api/fiscal/invoices/{sourceSystem}/{sourceReference}

Returns the stored result for an invoice (for reprints). Same body as the 200 response above. 404 if not found. If the invoice exists but is not signed, returns 409 with its current status.

### POST /api/fiscal/invoices/{docNum}

Legacy manual flow for EMA. Reads the invoice from SAP by DocNum. Takes no body. Requires an EMA (or `*`) key. It still uses placeholders (cashier "Test Cashier", date = now, payment type "Other"), so it is for testing only.

## Pending invoices and recovery

If the SDC does not answer in time, the invoice is stored as **Pending**. The outcome is unknown, because the SDC may have signed it. While an invoice is Pending:

- Repeating the call returns 409 and does NOT resend it to the SDC.
- Callers must not print.

A background worker (`PendingRecoveryWorker`) sweeps Pending rows and asks the SDC for the result using the row's `RequestId` (a read-only GET, it never sends invoices):

| SDC answer | Worker action |
|------------|---------------|
| Has the invoice | Copies the signed result into the row and marks it **Signed**. |
| Has no record | Marks the row **Failed** ("safe to resend"). The caller's next retry sends it. |
| Unreachable, empty or unreadable answer | Leaves the row Pending and tries again on the next sweep. |

The worker only looks at rows older than `Recovery:MinAgeSeconds`, so it never touches an invoice that is still in flight. An empty or `null` SDC answer is treated as "cannot tell", never as "not found", because that could cause a duplicate fiscal invoice.

**Resending is deliberately left to the caller.** Auto-resending could fiscalise a sale the cashier already abandoned.

> **Verify before relying on this:** the SDC lookup route (`GET v4/invoices/{requestId}`) must be confirmed against the Developer Portal documentation. In the sandbox it returned `null` for an invoice that had been signed. Until the correct lookup is confirmed, treat long-lived Pending rows as needing manual checking in the ERS/SDC portal.

Worker settings:

| Setting | Default | Meaning |
|---------|---------|---------|
| `Recovery:IntervalSeconds` | 60 | Time between sweeps |
| `Recovery:MinAgeSeconds` | 120 | Minimum age of a Pending row before it is checked |

## Configuration

| Setting | Notes |
|---------|-------|
| `Sdc:BaseUrl` | DevSDC URL from the Developer Portal, ending in `/api/` (keep the trailing slash). |
| `Sdc:Pin` | SDC PIN. Secret. |
| `TaxLabels` | Maps SAP VAT group to SDC tax label, case sensitive. Labels differ per environment. |
| `ApiKeys:N:Name` / `Key` / `SourceSystem` | One entry per caller. Secret. `SourceSystem` is `EMA`, `MACMOBILE` or `*`. |
| `Recovery:IntervalSeconds`, `Recovery:MinAgeSeconds` | Worker timing. |
| Database connection string | PostgreSQL, plus the SAP connection used by the legacy flow. |

Secrets (`Sdc:Pin`, `ApiKeys`, connection strings) are never committed to git.

- **Development:** `dotnet user-secrets set "Key" 'value'`. Use **single quotes** in PowerShell and bash so characters like `$`, `!` and `|` are not expanded. User-secrets override `appsettings.json` in Development, and changes apply after a restart.
- **Production:** environment variables (see below). In environment variables a `:` becomes `__`, for example `ApiKeys__0__Key`.

## Database

PostgreSQL. Table `FiscalInvoices`. Unique indexes: `("SourceSystem", "SourceReference", "DocType")` and `"RequestId"`. `RequestId` is `EMA-<ref>` or `MM-<ref>`, max 32 characters (hashed if longer). Status is stored as text (`Pending`, `Signed`, `Failed`).

## Production deployment (Linux)

Layout: **Kestrel** listens on localhost only, **nginx** terminates HTTPS in front of it, and **systemd** runs and restarts the app. Callers reach the API over HTTPS only. Never expose port 5000 or accept plain HTTP, because the API key travels in a header.

### 1. Publish

```bash
dotnet publish -c Release -o /opt/ers-fiscal
sudo useradd --system --no-create-home --shell /usr/sbin/nologin ersfiscal
sudo chown -R ersfiscal:ersfiscal /opt/ers-fiscal
```

### 2. Generate one key per caller

Keys are not issued by any service. You create them yourself as long random strings, one per caller:

```bash
openssl rand -hex 32     # run once for EMA, once per Macmobile caller
```

Use hex keys like this in production. They contain no special characters, so they cannot be mangled by a shell or by the env file. Give each key to its owner privately (in person or via a password manager), never by email or chat.

### 3. Secrets file

Create `/etc/ers-fiscal/ers-fiscal.env`, readable only by root (systemd reads it as root before dropping privileges):

```bash
sudo mkdir -p /etc/ers-fiscal
sudo nano /etc/ers-fiscal/ers-fiscal.env
sudo chown root:root /etc/ers-fiscal/ers-fiscal.env
sudo chmod 600 /etc/ers-fiscal/ers-fiscal.env
```

Contents:

```ini
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://127.0.0.1:5000

Sdc__BaseUrl=<production SDC URL ending in /api/>
Sdc__Pin=<sdc pin>

ApiKeys__0__Name=EMA
ApiKeys__0__Key=<hex key 1>
ApiKeys__0__SourceSystem=EMA

ApiKeys__1__Name=Macmobile-A
ApiKeys__1__Key=<hex key 2>
ApiKeys__1__SourceSystem=MACMOBILE

ApiKeys__2__Name=Macmobile-B
ApiKeys__2__Key=<hex key 3>
ApiKeys__2__SourceSystem=MACMOBILE

# Use the connection string names your Program.cs reads, for example:
ConnectionStrings__<FiscalDb>=Host=localhost;Database=<db>;Username=<user>;Password=<password>
ConnectionStrings__<SapDb>=<sap connection string>

Recovery__IntervalSeconds=60
Recovery__MinAgeSeconds=120
```

The `TaxLabels` section can stay in `appsettings.Production.json` (it holds no secrets) or be set the same way as `TaxLabels__S1=<label>`.

### 4. systemd service

`/etc/systemd/system/ers-fiscal.service` (change the DLL name to your published assembly):

```ini
[Unit]
Description=ERS Fiscalisation API
After=network.target postgresql.service

[Service]
WorkingDirectory=/opt/ers-fiscal
ExecStart=/usr/bin/dotnet /opt/ers-fiscal/ERS_Intergration.dll
EnvironmentFile=/etc/ers-fiscal/ers-fiscal.env
User=ersfiscal
Restart=always
RestartSec=5
KillSignal=SIGINT
TimeoutStopSec=30
SyslogIdentifier=ers-fiscal

[Install]
WantedBy=multi-user.target
```

```bash
sudo systemctl daemon-reload
sudo systemctl enable --now ers-fiscal
sudo systemctl status ers-fiscal
journalctl -u ers-fiscal -f        # live logs
```

### 5. nginx with HTTPS

```nginx
server {
    listen 80;
    server_name fiscal.example.com;
    return 301 https://$host$request_uri;
}

server {
    listen 443 ssl;
    server_name fiscal.example.com;

    ssl_certificate     /etc/ssl/certs/fiscal.crt;      # e.g. Let's Encrypt or an internal CA
    ssl_certificate_key /etc/ssl/private/fiscal.key;

    client_max_body_size 1m;

    location / {
        proxy_pass http://127.0.0.1:5000;
        proxy_set_header Host $host;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_read_timeout 90s;
    }
}
```

Keep `proxy_read_timeout` **longer than the app's SDC HttpClient timeout**. If nginx gives up first, the caller gets a 504 instead of the 409 Pending response, and will not know the invoice may have been signed.

### 6. Firewall and network

- Open only 443 (and 80 for the redirect) to callers.
- Block 5000 from outside. It is bound to 127.0.0.1 already, so this is a second layer.
- The server must be able to reach the SDC and the PostgreSQL/SAP databases.

### 7. Check it works

```bash
curl -i https://fiscal.example.com/api/fiscal/invoices/EMA/0                  # expect 401 (no key)
curl -i -H "X-Api-Key: <ema key>" https://fiscal.example.com/api/fiscal/invoices/EMA/0   # expect 404 (valid key, no such invoice)
curl -i -H "X-Api-Key: <ema key>" https://fiscal.example.com/api/fiscal/invoices/MACMOBILE/0   # expect 403
```

### Adding, revoking and rotating keys

Configuration is read at startup, so every change below ends with `sudo systemctl restart ers-fiscal`.

- **Add a caller:** add the next `ApiKeys__N__...` entries and restart.
- **Revoke a caller:** delete its three lines and restart. Other callers are unaffected.
- **Rotate a key:** add the new key as an extra entry for the same caller, give it to them, wait until they have switched, then remove the old entry and restart.

Restart outside business hours where possible: a request in flight during a restart may end up Pending, which the caller handles by retrying the same call.

## Not covered yet

- Refunds, credit notes, debit notes, cancellations and B2B invoices.
- Rate limiting on the API.
- Confirmed SDC lookup for Pending recovery (see the note under Pending invoices and recovery).
- Offline invoicing. ERS likely does not allow invoices without an SDC. This is a business decision.
