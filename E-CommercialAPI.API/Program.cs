using E_CommercialAPI.Application;
using E_CommercialAPI.Application.Validators.Products;
using E_CommercialAPI.Infrastructure;
using E_CommercialAPI.Infrastructure.Enums;
using E_CommercialAPI.Infrastructure.Filters;
using E_CommercialAPI.Infrastructure.Services.Storage.Azure;
using E_CommercialAPI.Infrastructure.Services.Storage.Local;
using E_CommercialAPI.Persistance;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistanceServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

//builder.Services.AddStorage<AzureStorage>();
builder.Services.AddStorage<LocalStorage>();
//builder.Services.AddStorage(StorageType.Local);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy=>
    policy.WithOrigins("http://localhost:3000", "https://localhost:3000")));

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>())
    .ConfigureApiBehaviorOptions(opt=>opt.SuppressModelStateInvalidFilter = true);

//builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();