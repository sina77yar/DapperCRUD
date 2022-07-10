using Dapper;
using FirstAppWithDapper.Common;
using FirstAppWithDapper.Interfaces;
using FirstAppWithDapper.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAppWithDapper.Services
{
    public class ProductService : IProductService
    {
        //private IConfiguration configuration;
        //public ProductService(IConfiguration configuration)
        //{
        //    this.configuration = configuration;
        //}
        private DapperUtility dapperUtility;
        public ProductService(DapperUtility dapperUtility)
        {
            this.dapperUtility = dapperUtility;
        }
        public async Task<List<ProductVM>> GetAsync()
        {
            //1-Connection String
           // var ConnectionStr = configuration.GetConnectionString("NorthwindConnection");
            //2-SqlQuery Or StoredProcedure
            var query = @"Select ProductID,ProductName,UnitPrice,P.CategoryID,C.CategoryName,P.SupplierID,CompanyName
                        From Products as P
                        Inner Join Categories as C on P.CategoryID=C.CategoryID
                        Inner Join Suppliers as S on P.SupplierID=S.SupplierID;";
            //3-Create Instance IDbConnection
          //  IDbConnection dbConnection = new SqlConnection(ConnectionStr);
            IDbConnection dbConnection = dapperUtility.GetConnection();
            var result = await dbConnection.QueryAsync<ProductVM>(query);
            return result.ToList();
        }

        //برای آپدیت
        public async Task<ProductVM> GetByIdAsync(int id)
        {
            var query = @"Select ProductId,ProductName,CategoryId
                         ,SupplierId,UnitPrice From Products
                          Where ProductId=@ProductId";
            using (var con = dapperUtility.GetConnection())
            {
                return await con.QuerySingleOrDefaultAsync<ProductVM>(query, new { ProductID = id });
            }
        }

        public async Task AddAsync(ProductVM model)
        {
            var query = @"Insert Products(ProductName,CategoryID,SupplierID,UnitPrice)
                       VALUES (@ProductName,@CategoryID,@SupplierID,@UnitPrice)";
            using (var con = dapperUtility.GetConnection())
            {
                await con.ExecuteAsync(query, model);
            }
        }
        public async Task AddAsyncWithSP(ProductVM model)
        {
            var query = @"SP_Product_Insert";
            using (var con=dapperUtility.GetConnection())
            {
                //اگر ویومدلمون یا نام ها یکی نبود با دیتابیس یا مشکلی بود میتوان به صورت زیر پارامتر به پارامتر مشخص کرد 
                // و به جای کوئری، پارام را جایگزین کرده
                //var param = new DynamicParameters();
                //param.Add("ProductName", model.ProductName);
                //param.Add("CategoryId", model.CategoryId);
                //param.Add("SupplierId", model.SupplierId);
                //param.Add("UnitPrice", model.UnitPrice);
                await con.ExecuteAsync(query, /*param*//*Ya*/model, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task UpdateAsync (ProductVM model)
        {
            var query = @"SP_Product_Update";
            using (var con = dapperUtility.GetConnection())
            {
                var param = new DynamicParameters();
                param.Add("ProductName", model.ProductName);
                param.Add("CategoryId", model.CategoryId);
                param.Add("SupplierId", model.SupplierId);
                param.Add("UnitPrice", model.UnitPrice);
                param.Add("ProductID", model.ProductId);
                await con.ExecuteAsync(query, param, commandType: CommandType.StoredProcedure);
            }
        }
        public async Task DeleteAsync(int id)
        {
            var query = @"Delete Products where ProductID=@id";
            using (var con=dapperUtility.GetConnection())
            {
                await con.ExecuteAsync(query,new {id=id });
            }
        }

        public async Task BulkAddAsync(List<ProductVM> products)
        {
            var query = @"Insert Products(ProductName,CategoryID,SupplierID,UnitPrice)
                       VALUES (@ProductName,@CategoryID,@SupplierID,@UnitPrice)";
            using (var con = dapperUtility.GetConnection())
            {
                await con.ExecuteAsync(query, products);
            }
        }


    }
}
