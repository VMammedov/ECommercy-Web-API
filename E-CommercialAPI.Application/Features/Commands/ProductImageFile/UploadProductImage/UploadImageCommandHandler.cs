using E_CommercialAPI.Application.Features.Commands.ProductImageFile.UploadProductImageFile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadImageCommandHandler : IRequestHandler<UploadImageCommandRequest, UploadImageCommandResponse>
    {
        public async Task<UploadImageCommandResponse> Handle(UploadImageCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
