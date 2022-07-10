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
    public class SupplierService : ISupplierService
    {
        private DapperUtility DapperUtility;
        public SupplierService(DapperUtility dapperUtility)
        {
            this.DapperUtility = dapperUtility;
        }

        public async Task<List<SupplierVM>> GetSupplierForComboAsync()
        {
            using (var con = DapperUtility.GetConnection())
            {
                var query = @"Select SupplierID,CompanyName from Suppliers";
                var result = await con.QueryAsync<SupplierVM>(query);
                return result.ToList();
            }
        }
    }
}
