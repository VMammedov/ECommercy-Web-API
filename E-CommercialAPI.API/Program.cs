using E_CommercialAPI.API.Configurations.ColumnWriters;
using E_CommercialAPI.Application;
using E_CommercialAPI.Application.Validators.Products;
using E_CommercialAPI.Infrastructure;
using E_CommercialAPI.Infrastructure.Enums;
using E_CommercialAPI.Infrastructure.Filters;
using E_CommercialAPI.Infrastructure.Services.Storage.Azure;
using E_CommercialAPI.Infrastructure.Services.Storage.Local;
using E_CommercialAPI.Persistance;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistanceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

//builder.Services.AddStorage<AzureStorage>();
builder.Services.AddStorage<LocalStorage>();
//builder.Services.AddStorage(StorageType.Local);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy=>
    policy.WithOrigins("http://localhost:3000", "https://localhost:3000")));

// Activate Serilog
Serilog.Core.Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"), "logs", needAutoCreateTable : true,
    columnOptions: new Dictionary<string, ColumnWriterBase>
    {
        {"message", new RenderedMessageColumnWriter() },
        {"message_template", new MessageTemplateColumnWriter() },
        {"level", new LevelColumnWriter() },
        {"time_stamp", new TimestampColumnWriter() },
        {"exception", new ExceptionColumnWriter() },
        {"log_level", new LogEventSerializedColumnWriter() },
        {"user_name", new UserNameColumnWriter() }
    })
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

}); // httploggin ile http sorgularinin ve istifadeci melumatlarinin loglarini aliriq

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(opt=>opt.SuppressModelStateInvalidFilter = true);

//builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
    AddJwtBearer("Admin",options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true, // olusturulacak token degerini kimlerin (hangi originlerin, sitelerin) kullanacagini belirledigimiz degerdir. -> www.bilmemne.com
        ValidateIssuer = true, // olsuturulacak token degerini kimin dagittigini ifade edecegimiz alandir. -> www.myapi.com
        ValidateLifetime = true, // olsuturulacak token degerinin suresini kontrol edecek olan dogrulamadir.
        ValidateIssuerSigningKey = true, // (Simmetric key, Security key) olsuturulacak token degerinin uygulamamiza ait bir deger oldugunu ifade eden security key verisinin dogrulanmasidir.

        // Token dogrulandigi zaman yuxaridaki true dediyim deyerlere gore dogrulanacaq ve bu deyerlerin qiymetide asagidakilar oalcaq

        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
        
        NameClaimType = ClaimTypes.Name // JWT uzerinde Name claimine qarsiliq gelen deyeri User.Identity.Name propertysinden elde ede bilerik.
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseSerilogRequestLogging(); // bunu neyin loglanmasini isteyirikse ondan evvel yaziriq
app.UseHttpLogging();

app.UseCors();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) => // bu middleware sayesinde biz araya girib JWT deki user Name melumatini log mexanizminin contextine yeni property olaraq elave edirik
{
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name", username);

    await next();
});

app.MapControllers();

app.Run();