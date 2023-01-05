using E_CommercialAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P = E_CommercialAPI.Domain.Entities;

namespace E_CommercialAPI.Application.Features.Queries.Product.GetByIdProduct
{
    public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
    {
        IProductReadRepository _productReadRepository;

        public GetByIdProductQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
        {
             P.Product data = await _productReadRepository.GetByIdAsync(request.Id, false);

            return new GetByIdProductQueryResponse
            {
                Id = data.Id,
                Name = data.Name,
                Price = data.Price,
                CreatedDate = data.CreatedDate,
                Stock = data.Stock
            };
        }
    }
}
