using FirstAppWithDapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAppWithDapper.Interfaces
{
    public interface ISupplierService
    {
       Task<List<SupplierVM>> GetSupplierForComboAsync();
    }
}
