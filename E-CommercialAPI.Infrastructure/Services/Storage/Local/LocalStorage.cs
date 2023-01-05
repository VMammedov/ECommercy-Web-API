using E_CommercialAPI.Application.Abstractions.Storage.Local;
using E_CommercialAPI.Application.Repositories;
using E_CommercialAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : StorageCommon, ILocalStorage
    {
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IFileReadRepository _fileReadRepository;

        public LocalStorage(IWebHostEnvironment webHostEnviroment, IFileReadRepository fileReadRepository) : base(fileReadRepository)
        {
            _webHostEnviroment = webHostEnviroment;
            _fileReadRepository = fileReadRepository;
        }

        public async Task DeleteAsync(string path, string fileName)
        {
            File.Delete($"{_webHostEnviroment.WebRootPath}\\{path}\\{fileName}");
        }

        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            return directory.GetFiles().Select(f => f.Name).ToList();
        }

        public bool HasFile(string path, string fileName)
        {
            return File.Exists($"{_webHostEnviroment.WebRootPath}\\{path}\\{fileName}");
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnviroment.WebRootPath, path);

            List<(string fileName, string pathOrContainerName)> datas = new();

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(path, file.FileName, HasFile);
                await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);
                datas.Add((fileNewName, $"{path}\\{fileNewName}"));
            }

            return datas;
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, false);

                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                //todo log`
                throw ex;
            }
        }
    }
}
