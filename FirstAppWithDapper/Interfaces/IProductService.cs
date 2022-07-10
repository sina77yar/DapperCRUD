using FirstAppWithDapper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAppWithDapper.Interfaces
{
    public interface IProductService
    {
       Task<List<ProductVM>> GetAsync();
        Task AddAsync(ProductVM model);
        Task BulkAddAsync(List<ProductVM> products);
        Task AddAsyncWithSP(ProductVM model);
        Task <ProductVM> GetByIdAsync(int id);
        
        Task UpdateAsync(ProductVM model);
        Task DeleteAsync(int id);
    }
}
