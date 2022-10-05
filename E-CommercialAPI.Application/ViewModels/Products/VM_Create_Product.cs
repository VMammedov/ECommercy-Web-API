using E_CommercialAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Application.ViewModels.Products
{
    public class VM_Create_Product : BaseEntity
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
    }
}
