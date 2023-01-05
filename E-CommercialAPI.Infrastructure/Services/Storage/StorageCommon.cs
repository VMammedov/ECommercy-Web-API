using E_CommercialAPI.Application.Repositories;
using E_CommercialAPI.Infrastructure.Operations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Infrastructure.Services.Storage
{
    public class StorageCommon //todo public to protected
    {
        private readonly IFileReadRepository _fileReadRepository;

        public StorageCommon(IFileReadRepository fileReadRepository)
        {
            _fileReadRepository = fileReadRepository;
        }

        protected delegate bool HasFileMethod(string pathOrContainerName, string fileName);

        protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName, HasFileMethod hasFileMethod) // todo alqortim deyisilecek cunki faylin adinda 1 tire olarsa run time zamani xeta cixacaq
        {
            string extension = Path.GetExtension(fileName);
            string oldName = Path.GetFileNameWithoutExtension(fileName);
            string regulatedFileName = NameOperation.CharacterRegulator(oldName);

            if (!hasFileMethod(pathOrContainerName, fileName))
                return regulatedFileName + extension; //Demek ki bu isimde ilk kez dosya yükleniyor.

            var files = await _fileReadRepository.GetWhere(x => x.FileName.StartsWith(regulatedFileName)).Select(y=>y.FileName).ToListAsync();

            int[] fileNumbers = new int[files.Count];
            int lastHyphenIndex;
            for (int i = 0; i < files.Count; i++)
            {
                lastHyphenIndex = files[i].LastIndexOf("-");
                string str = lastHyphenIndex == -1 ? "0" : files[i].Substring(lastHyphenIndex + 1, files[i].Length - extension.Length - lastHyphenIndex - 1);
                fileNumbers[i] = int.Parse(str);
            }
            var biggestNumber = fileNumbers.Max(); //en yüksek sayıyı bulduk
            biggestNumber++;
            return $"{regulatedFileName}-{biggestNumber}{extension}"; //bir artırıp dönüyoruz
        }
    }
}
