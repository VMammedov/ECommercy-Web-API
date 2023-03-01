using E_CommercialAPI.Application.Abstractions.Storage;
using E_CommercialAPI.Application.Features.Commands.Product.CreateProduct;
using E_CommercialAPI.Application.Features.Commands.Product.DeleteProduct;
using E_CommercialAPI.Application.Features.Commands.Product.UpdateProduct;
using E_CommercialAPI.Application.Features.Commands.ProductImageFile.DeleteProductImage;
using E_CommercialAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;
using E_CommercialAPI.Application.Features.Commands.ProductImageFile.UploadProductImageFile;
using E_CommercialAPI.Application.Features.Queries.Product.GetAllProducts;
using E_CommercialAPI.Application.Features.Queries.Product.GetByIdProduct;
using E_CommercialAPI.Application.Features.Queries.ProductImageFile.GetProductImages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_CommercialAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class ProductsController : ControllerBase
    {
        readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductsQueryRequest getAllProductsQueryRequest)
        {
            GetAllProductsQueryResponse response = await _mediator.Send(getAllProductsQueryRequest);

            return Ok(response);
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
        public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteProductCommandRequest deleteProductCommandRequest)
        {
            DeleteProductCommandResponse response = await _mediator.Send(deleteProductCommandRequest);
            return Ok();
        }

        [HttpPost("[action]")] // burada id query stringden gelir yeni abc.com/Upload?id=123 - ferqleri oduki men bilmirem upload-a nece deyer gele biler. Meselen sabah video yuklesem onun vaxti filanda gonderile biler.
        public async Task<IActionResult> Upload([FromQuery] UploadImageCommandRequest uploadImageCommandRequest)
        {
            uploadImageCommandRequest.FormFiles = Request.Form.Files;
            UploadImageCommandResponse response = await _mediator.Send(uploadImageCommandRequest);
            return Ok();
        }

        [HttpGet("[action]/{Id}")] // burada route parametrinden gonderirik. Sebebde odur ki, her zaman bilirem ki, hansisa product-in sekillerine ehtiyacim olsa onun id-sini gondermeyim yetir
        public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest)
        {
            GetProductImagesQueryResponse response = await _mediator.Send(getProductImagesQueryRequest);
            if (response.TotalCount > 0)
                return Ok(response);
            else
                return Ok("No data aviable");
        }

        [HttpDelete("[action]/{productId}/{imageId}")] // parametrler bu adlarla eyni olmalidir
        public async Task<IActionResult> DeleteProductImage([FromRoute] DeleteProductImageCommandRequest deleteProductImageCommandRequest)
        {
            DeleteProductImageCommandResponse response = await _mediator.Send(deleteProductImageCommandRequest);
            return Ok();
        }

        //[HttpGet("[action]")]
        //public async Task<IActionResult> GetFiles()
        //{
        //    var datas = _fileReadRepository.GetAll(false);
        //    return Ok(datas);
        //}

        //[HttpDelete("[action]/{Id}")]
        //public async Task<IActionResult> FileDelete()
        //{
        //    await _fileWriteRepository.RemoveAsync(id);
        //    await _fileWriteRepository.SaveAsync();
        //    return Ok();
        //}

    }
}