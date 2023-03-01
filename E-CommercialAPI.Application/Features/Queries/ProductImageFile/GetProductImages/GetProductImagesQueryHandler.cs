using E_CommercialAPI.Application.Repositories;
using E_CommercialAPI.Application.ViewModels.ProductImages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity = E_CommercialAPI.Domain.Entities;

namespace E_CommercialAPI.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQueryRequest, GetProductImagesQueryResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IConfiguration _configuration;

        public GetProductImagesQueryHandler(IProductReadRepository productReadRepository, IConfiguration configuration)
        {
            _productReadRepository = productReadRepository;
            _configuration = configuration;
        }

        public async Task<GetProductImagesQueryResponse> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
        {
            entity.Product? product = await _productReadRepository.Table.Include(x => x.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));
            GetProductImagesQueryResponse response = new GetProductImagesQueryResponse();

            if (product is not null && product.ProductImageFiles is not null)
            {
                response.ProductImages = new List<VM_Get_ProductImage>();
                foreach (entity.ProductImageFile item in product.ProductImageFiles)
                {
                    response.ProductImages.Add(new VM_Get_ProductImage()
                    {
                        Id = item.Id,
                        Path = $"{_configuration["BaseStorageUrl"]}/{item.Path}",
                        FileName = item.FileName
                    });
                }
                response.TotalCount = response.ProductImages.Count;
            }

            return response;
        }
    }
}
