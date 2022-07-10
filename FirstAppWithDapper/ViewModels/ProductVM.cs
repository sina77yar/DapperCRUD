using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAppWithDapper.ViewModels
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }

        public double? UnitPrice { get; set; }
    }
}
