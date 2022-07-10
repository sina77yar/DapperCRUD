using Dapper;
using FirstAppWithDapper.Common;
using FirstAppWithDapper.Interfaces;
using FirstAppWithDapper.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAppWithDapper.Services
{
    public class CategoryService:ICategoryService
    {
        private DapperUtility DapperUtility;
        public CategoryService(DapperUtility dapperUtility)
        {
            this.DapperUtility= dapperUtility;
        }

        public async Task<List<CategoryVM>> GetCategoryForComboAsync()
        {
            using(var con=DapperUtility.GetConnection())
            {
                var query = @"Select CategoryId,CategoryName from Categories";
                var result= await con.QueryAsync<CategoryVM>(query);
                return result.ToList();
            }
        }
    }
}
