using E_CommercialAPI.Application.Repositories;
using E_CommercialAPI.Application.RequestParameters;
using E_CommercialAPI.Application.ViewModels.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
    {
        IProductReadRepository _productReadRepository;
        public GetAllProductsQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            int totalCount = _productReadRepository.GetAll().Count();
            var products = _productReadRepository.GetAll()
                .Select(x => new VM_GetAll_Product
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Stock = x.Stock,
                    CreatedDate = x.CreatedDate
                }).Skip(request.Page * request.Size).Take(request.Size).ToList();

            return new GetAllProductsQueryResponse { TotalCount = totalCount, Products = products };
        }
    }
}
