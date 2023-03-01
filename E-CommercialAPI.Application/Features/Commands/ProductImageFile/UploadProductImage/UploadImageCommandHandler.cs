using E_CommercialAPI.Application.Abstractions.Storage;
using E_CommercialAPI.Application.Features.Commands.ProductImageFile.UploadProductImageFile;
using E_CommercialAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity = E_CommercialAPI.Domain.Entities;

namespace E_CommercialAPI.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadImageCommandHandler : IRequestHandler<UploadImageCommandRequest, UploadImageCommandResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IStorageService _storageService;

        public UploadImageCommandHandler(IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService)
        {
            _productReadRepository = productReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _storageService = storageService;
        }

        public async Task<UploadImageCommandResponse> Handle(UploadImageCommandRequest request, CancellationToken cancellationToken)
        {
            #region OldCode
            //wwwroot/resource/product
            //string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "resource/product-images");

            //if (!Directory.Exists(uploadPath))
            //    Directory.CreateDirectory(uploadPath);

            //foreach (IFormFile file in formFiles)
            //{
            //    string fullPath = Path.Combine(uploadPath, $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}");

            //    using (FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024))
            //    {
            //        await file.CopyToAsync(fileStream);
            //        await fileStream.FlushAsync();
            //    }
            //}
            #endregion
            #region OldCode2
            //await _invoiceFileWriteRepository.AddRangeAsync(datas.Select(x => new InvoiceFile
            //{
            //    FileName = x.fileName,
            //    Path = x.path,
            //    Price = new Random().Next(1,100)
            //}).ToList());

            //await _invoiceFileWriteRepository.SaveAsync();

            //await _fileWriteRepository.AddRangeAsync(datas.Select(x => new E_CommercialAPI.Domain.Entities.File
            //{
            //    FileName = x.fileName,
            //    Path = x.path
            //}).ToList());

            //await _fileWriteRepository.SaveAsync();

            //List<(string fileName, string pathOrContainerName)> datas = await _storageService.UploadAsync("product-images", formFiles);

            //await _productImageFileWriteRepository.AddRangeAsync(datas.Select(x => new ProductImageFile
            //{
            //    FileName = x.fileName,
            //    Path = x.pathOrContainerName,
            //    Storage = _storageService.StorageName
            //}).ToList());

            //await _productImageFileWriteRepository.SaveAsync();

            #endregion

            List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images", request.FormFiles);
            entity.Product product = await _productReadRepository.GetByIdAsync(request.Id);

            bool addDbResult = await _productImageFileWriteRepository.AddRangeAsync(result.Select(x => new entity.ProductImageFile
            {
                FileName = x.fileName,
                Path = x.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<entity.Product>() { product }
            }).ToList());

            ////alternative of above code
            //foreach (var item in result)
            //{
            //    product.ProductImageFiles.Add(new ProductImageFile(){
            //        FileName = item.fileName,
            //        Path = item.pathOrContainerName,
            //        Storage = _storageService.StorageName,
            //        Products = new List<Product>() { product }
            //    });
            //}

            await _productImageFileWriteRepository.SaveAsync();

            return new UploadImageCommandResponse();
        }
    }
}
