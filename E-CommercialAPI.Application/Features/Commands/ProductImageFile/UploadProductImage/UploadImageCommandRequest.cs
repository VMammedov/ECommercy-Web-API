using E_CommercialAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Application.Features.Commands.ProductImageFile.UploadProductImageFile
{
    public class UploadImageCommandRequest : IRequest<UploadImageCommandResponse>
    {
        public string Id { get; set; }
        public IFormFileCollection FormFiles { get; set; }
    }
}
