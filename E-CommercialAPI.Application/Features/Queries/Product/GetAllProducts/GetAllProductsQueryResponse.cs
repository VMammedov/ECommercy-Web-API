using E_CommercialAPI.Application.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsQueryResponse
    {
        public int TotalCount { get; set; }
        public List<VM_GetAll_Product> Products { get; set; }
    }
}
