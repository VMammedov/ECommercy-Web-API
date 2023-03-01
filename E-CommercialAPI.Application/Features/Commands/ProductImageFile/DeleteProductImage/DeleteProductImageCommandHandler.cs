using E_CommercialAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entity = E_CommercialAPI.Domain.Entities;

namespace E_CommercialAPI.Application.Features.Commands.ProductImageFile.DeleteProductImage
{
    public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommandRequest, DeleteProductImageCommandResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductWriteRepository _productWriteRepository;

        public DeleteProductImageCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        public async Task<DeleteProductImageCommandResponse> Handle(DeleteProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            entity.Product? product = await _productReadRepository.Table.Include(x => x.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.productId));

            if (product is not null)
            {
                entity.ProductImageFile? productImageFile = product.ProductImageFiles.FirstOrDefault(x => x.Id == Guid.Parse(request.imageId));
                if (productImageFile is not null)
                {
                    product.ProductImageFiles.Remove(productImageFile);
                    await _productWriteRepository.SaveAsync();
                }
            }

            return new DeleteProductImageCommandResponse();
        }
    }
}
