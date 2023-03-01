using E_CommercialAPI.Application.ViewModels.ProductImages;
using E_CommercialAPI.Application.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryResponse
    {
        public int TotalCount { get; set; }
        public List<VM_Get_ProductImage> ProductImages { get; set; }
    }
}
