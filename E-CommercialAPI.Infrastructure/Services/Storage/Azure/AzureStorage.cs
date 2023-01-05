using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using E_CommercialAPI.Application.Abstractions.Storage.Azure;
using E_CommercialAPI.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : StorageCommon, IAzureStorage
    {
        readonly BlobServiceClient _blobServiceClient; // Azure storage account-na baglanmaq ucun
        BlobContainerClient _blobContainerClient; // O account-daki hedef container uzerinde fayl emeliyyatlari etmeyimize komek edir
        private readonly IFileReadRepository _fileReadRepository;

        public AzureStorage(IConfiguration configuration, IFileReadRepository fileReadRepository) : base(fileReadRepository)
        {
            _blobServiceClient = new(configuration["Storage:Azure"]);
            _fileReadRepository = fileReadRepository;
        }

        public async Task DeleteAsync(string ContainerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string ContainerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            return _blobContainerClient.GetBlobs().Select(x=>x.Name).ToList();
        }

        public bool HasFile(string ContainerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            return _blobContainerClient.GetBlobs().Any(x => x.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string ContainerName, IFormFileCollection files)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            List<(string fileName, string pathOrContainerName)> datas = new();
            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(ContainerName, file.FileName, HasFile);

                BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName);
                await blobClient.UploadAsync(file.OpenReadStream());
                datas.Add((fileNewName, $"{ContainerName}/{fileNewName}"));
            }

            return datas;
        }
    }
}
