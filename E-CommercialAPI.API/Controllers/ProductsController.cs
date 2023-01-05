using E_CommercialAPI.Application.Abstractions.Storage;
using E_CommercialAPI.Application.Features.Commands.Product.CreateProduct;
using E_CommercialAPI.Application.Features.Queries.Product.GetAllProducts;
using E_CommercialAPI.Application.Features.Queries.Product.GetByIdProduct;
using E_CommercialAPI.Application.Repositories;
using E_CommercialAPI.Application.RequestParameters;
using E_CommercialAPI.Application.ViewModels.Products;
using E_CommercialAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace E_CommercialAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        private readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        private readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        readonly IStorageService _storageService;
        readonly IConfiguration _configuration;

        readonly IMediator _mediator;

        public ProductsController(IProductReadRepository productReadRepository,
            IProductWriteRepository productWriteRepository,
            IWebHostEnvironment webHostEnvironment,
            IFileReadRepository fileReadRepository, IFileWriteRepository fileWriteRepository, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IInvoiceFileReadRepository invoiceFileReadRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository, IStorageService storageService, IConfiguration configuration, IMediator mediator)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnvironment = webHostEnvironment;
            _fileReadRepository = fileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _storageService = storageService;
            _configuration = configuration;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductsQueryRequest getAllProductsQueryRequest)
        {
            GetAllProductsQueryResponse response = await _mediator.Send(getAllProductsQueryRequest);

            return Ok(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetFiles()
        {
            var datas = _fileReadRepository.GetAll(false);
            return Ok(datas);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest)
        {
            GetByIdProductQueryResponse response = await _mediator.Send(getByIdProductQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product productM)
        {
            Product product = await  _productReadRepository.GetByIdAsync(productM.Id);
            product.Name = productM.Name;
            product.Stock = productM.Stock;
            product.Price = productM.Price;
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> FileDelete(string id)
        {
            await _fileWriteRepository.RemoveAsync(id);
            await _fileWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpPost("[action]")] // burada id query stringden gelir yeni abc.com/Upload?id=123 - ferqleri oduki men bilmirem upload-a nece deyer gele biler. Meselen sabah video yuklesem onun vaxti filanda gonderile biler.
        public async Task<IActionResult> Upload(string id, [FromForm] IFormFileCollection formFiles)
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

            List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images", formFiles);

            Product product = await _productReadRepository.GetByIdAsync(id);

            bool addDbResult = await _productImageFileWriteRepository.AddRangeAsync(result.Select(x => new ProductImageFile
            {
                FileName = x.fileName,
                Path = x.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Product>() { product }
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

            return Ok();
        }

        [HttpGet("[action]/{id}")] // burada route parametrinden gonderirik. Sebebde odur ki, her zaman bilirem ki, hansisa product-in sekillerine ehtiyacim olsa onun id-sini gondermeyim yetir
        public async Task<IActionResult> GetProductImages(string id)
        {
            Product? product = await _productReadRepository.Table.Include(x => x.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            if (product is not null)
            {
                return Ok(product.ProductImageFiles.Select(p => new
                {
                    p.Id,
                    Path = $"{_configuration["BaseStorageUrl"]}/{p.Path}",
                    p.FileName
                }));
            }
            else
                return Ok("No data aviable");
        }

        [HttpDelete("[action]/{productId}/{imageId}")] // parametrler bu adlarla eyni olmalidir
        public async Task<IActionResult> DeleteProductImage(string productId, string imageId)
        {
            Product? product = await _productReadRepository.Table.Include(x => x.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(productId));

            if (product is not null)
            {
                ProductImageFile? productImageFile = product.ProductImageFiles.FirstOrDefault(x => x.Id == Guid.Parse(imageId));
                if (productImageFile is not null)
                {
                    product.ProductImageFiles.Remove(productImageFile);
                    await _productWriteRepository.SaveAsync();
                }
            }

            return Ok();
        }

    }
}