using System;
using System.Linq;
using Raven.Client.Documents;
using RavenDB.Tryout.App.Indexes;
using RavenDB.Tryout.App.Models;

namespace RavenDB.Tryout.App
{
    public static class Examples
    {
        public static void E1_LoadAndInclude()
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                var product = session
                    .Include<Product>(x => x.Supplier)
                    .Load<Product>("products/77-A");

                var supplier = session.Load<Supplier>(product.Supplier);
                
                Console.WriteLine($"Product name: {product.Name}");
                Console.WriteLine($"Supplier name: {supplier.Name}");
            }
            
// from Products as p
// load p.Category as c
// select {
//     ProductName: p.Name,
//     CategoryName: c.Name
// }
        }

        public static void E2_QueryIndexAndInclude()
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                var products = session
                    .Query<Product_Search.Result, Product_Search>()
                    .Include(x => x.Category)
                    .ToList();

                foreach (var product in products)
                {
                    var category = session.Load<Category>(product.Category);
                    Console.WriteLine($"{product.Name} ({category.Name})");
                }
            }
            
//from index 'Product/Search' as p
// load p.Category as c
// select {
//     ProductName: p.Name,
//     CategoryName: c.Name
// }
        }

        public static void E3_LoadDocumentInIndex()
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                var beverageProducts = session
                    .Query<Product_SearchCategoryName.Result, Product_SearchCategoryName>()
                    .Customize(x => x.WaitForNonStaleResults())
                    .Where(x => x.CategoryName == "Beverages")
                    .ProjectInto<Product_SearchCategoryName.Result>()
                    .ToList();

                foreach (var beverageProduct in beverageProducts)
                {
                    Console.WriteLine($"{beverageProduct.Name} ({beverageProduct.CategoryName})");
                }
            }
            
// from p in docs.Products
// let category = LoadDocument(p.Category, "Categories")
// select new {
//     Name = p.Name,
//     CategoryName = category.Name,
//     Supplier = p.Supplier,
//     PricePerUnit = p.PricePerUnit
// }

// from index 'Product/SearchCategoryName' as p
// where p.CategoryName = "Beverages"
// select {
//     ProductName: p.Name,
//     CategoryName: p.CategoryName
// }
            
        }

        public static void E4_LoadDocumentInIndex_NestedInSelect()
        {
            using (var session = DocumentStoreHolder.Store.OpenSession())
            {
                var ordersWithDiscontinuedProducts = session
                    .Query<Orders_ProductCont.Result, Orders_ProductCont>()
                    .Customize(x => x.WaitForNonStaleResults())
                    .Where(x => x.AnyProductDiscontinued == true)
                    .ProjectInto<Orders_ProductCont.Result>()
                    .ToList();

                foreach (var result in ordersWithDiscontinuedProducts)
                {
                    Console.WriteLine(result.Id);
                }
            }
        }
    }
}