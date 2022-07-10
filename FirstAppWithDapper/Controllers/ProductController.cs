using ExcelDataReader;
using FirstAppWithDapper.Common;
using FirstAppWithDapper.Interfaces;
using FirstAppWithDapper.Services;
using FirstAppWithDapper.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAppWithDapper.Controllers
{
    public class ProductController : Controller
    {
        private IProductService productService;
        private ICategoryService CategoryService;
        private ISupplierService supplierService;
        public ProductController(IProductService productService, ISupplierService supplierService, ICategoryService categoryService)
        {
            this.productService = productService;
            this.supplierService = supplierService;
            this.CategoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await productService.GetAsync();
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await CategoryService.GetCategoryForComboAsync();
            ViewBag.Suppliers = await supplierService.GetSupplierForComboAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateWithSP(ProductVM product)
        {
            if (ModelState.IsValid)
            {
                await productService.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Categories = await CategoryService.GetCategoryForComboAsync();
            ViewBag.Suppliers = await supplierService.GetSupplierForComboAsync();
            var Product = await productService.GetByIdAsync(id);
            return View(Product);
        }
        [HttpPost]
        public async Task<IActionResult> EditWithSP(ProductVM product)
        {
            if (ModelState.IsValid)
            {
                await productService.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();

        }


        public async Task<IActionResult> Delete(int id)
        {
            await productService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult BulkInsert()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BulkInsert(IFormFile excelfile)
        {
            var products = new List<ProductVM>();

            using (var ms = new MemoryStream())
            {
                excelfile.CopyTo(ms);

                using (var reader = ExcelReaderFactory.CreateReader(ms))
                {
                    //Choose One of either 1 or 2:
                    //1. Use the reader methods
                    do
                    {
                        while (reader.Read())
                        {
                            var product = new ProductVM();
                            if (reader[0].ToString().ToLower()=="ProductName".ToLower())
                            {
                                continue;
                            }
                            product.ProductName = reader[0].ToString();
                            product.UnitPrice = Convert.ToDouble(reader[3]);
                            product.CategoryId = Convert.ToInt32(reader[1]);
                            product.SupplierId = Convert.ToInt32(reader[2]);


                            products.Add(product);
                        }
                    } while (reader.NextResult());
                }
                if (products.Count>0)
                {
                    productService.BulkAddAsync(products);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
