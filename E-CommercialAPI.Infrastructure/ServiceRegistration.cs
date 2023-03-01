using E_CommercialAPI.Application.Abstractions.Storage;
using E_CommercialAPI.Application.Abstractions.Token;
using E_CommercialAPI.Infrastructure.Enums;
using E_CommercialAPI.Infrastructure.Services;
using E_CommercialAPI.Infrastructure.Services.Storage;
using E_CommercialAPI.Infrastructure.Services.Storage.Azure;
using E_CommercialAPI.Infrastructure.Services.Storage.Local;
using E_CommercialAPI.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();
            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
        }

        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : StorageCommon, IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        }

        public static void AddStorage(this IServiceCollection serviceCollection, StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Local:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    serviceCollection.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:
                    break;
                default:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }

        //public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : class, IStorage
        //{
        //    serviceCollection.AddScoped<IStorage, T>();
        //}
    }
}
