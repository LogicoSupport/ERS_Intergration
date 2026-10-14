using ERS_Intergration.DataContext;
using ERS_Intergration.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
 builder.Services.AddScoped<InvoiceMapper>();
builder.Services.AddScoped<ApiKeyFilter>();
builder.Services.AddHostedService<PendingRecoveryWorker>();
builder.Services.AddHttpClient<SdcClient>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["SDC:Url"]!.TrimEnd('/') + "/");
    c.Timeout = TimeSpan.FromSeconds(30);
    c.DefaultRequestHeaders.Accept.ParseAdd("application/json");
});
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<SapDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SAP")));
builder.Services.AddDbContext<FiscalDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("Fiscal")));
builder.Services.AddScoped<FiscalisationService>();
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });

    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("http://0.0.0.0:8080") // Specific origin for production
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
