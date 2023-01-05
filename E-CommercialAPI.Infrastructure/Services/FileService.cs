using E_CommercialAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace E_CommercialAPI.Infrastructure.Services
{
    public class FileService
    {
        private readonly IWebHostEnvironment _webHostEnviroment;

        public FileService(IWebHostEnvironment webHostEnviroment)
        {
            _webHostEnviroment = webHostEnviroment;
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

        //async Task<string> FileRenameAsync(string path, string fileName, bool first = true)
        //{
        //    string newFileName = await Task.Run<string>(async () =>
        //    {
        //        string extension = Path.GetExtension(fileName);
        //        string newFileName = string.Empty;

        //        if (first)
        //        {
        //            string oldName = Path.GetFileNameWithoutExtension(fileName);
        //            newFileName = $"{NameOperation.CharacterRegulator(fileName)}{extension}";
        //        }
        //        else
        //        {
        //            newFileName = fileName;
        //            int indexNo1 = newFileName.LastIndexOf("-");
        //            if(indexNo1 == -1)
        //            {
        //                newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
        //            }
        //            else
        //            {
        //                int indexNo2 = newFileName.LastIndexOf(".");
        //                string fileNo = newFileName.Substring(indexNo1, indexNo2 - indexNo1 - 1);

        //                if(int.TryParse(fileNo,out int _fileNo))
        //                {
        //                    int _fileNo = int.Parse(fileNo);
        //                    _fileNo++;
        //                }

        //                newFileName = newFileName.Remove(indexNo1, indexNo2 - indexNo1 - 1).Insert(indexNo1, _fileNo.ToString());
        //            }
        //        }

        //        if (File.Exists($"{path}\\{newFileName}"))
        //        {
        //            return await FileRenameAsync(path, newFileName, false);
        //        }
        //        else
        //            return newFileName;
        //    });

        //    return newFileName;
        //}

        async Task<string> FileRenameAsync(string path, string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string oldName = Path.GetFileNameWithoutExtension(fileName);
            string regulatedFileName = NameOperation.CharacterRegulator(oldName);

            var files = Directory.GetFiles(path, regulatedFileName + "*"); //bu isimle başlayan tüm dosyaları bulur

            if (files.Length == 0) return regulatedFileName + "-1" + extension; //Demek ki bu isimde ilk kez dosya yükleniyor.

            int[] fileNumbers = new int[files.Length];  //Dosya numaralarını buraya alıp en yükseğini bulucaz.
            int lastHyphenIndex;
            for (int i = 0; i < files.Length; i++)
            {
                lastHyphenIndex = files[i].LastIndexOf("-");
                fileNumbers[i] = int.Parse(files[i].Substring(lastHyphenIndex + 1, files[i].Length - extension.Length - lastHyphenIndex - 1));
            }
            var biggestNumber = fileNumbers.Max(); //en yüksek sayıyı bulduk
            biggestNumber++;
            return $"{regulatedFileName}-{biggestNumber}{extension}"; //bir artırıp dönüyoruz
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnviroment.WebRootPath, path);

            List<(string fileName, string path)> datas = new();
            List<bool> results = new();

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(uploadPath,file.FileName);

                bool result = await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);
                datas.Add((fileNewName, $"{path}\\{fileNewName}"));
                results.Add(result);
            }

            if (results.TrueForAll(r => r.Equals(true)))
                return datas;

            //todo If "if" statement that located on top is not valid, thorw new Exception about it
            return null;
        }
    }
}