using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Application.ViewModels.ProductImages
{
    public class VM_Get_ProductImage
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
    }
}
