using System.Linq;
using Raven.Client.Documents.Indexes;
using RavenDB.Tryout.App.Models;

namespace RavenDB.Tryout.App.Indexes
{
    public class Product_SearchCategoryName : AbstractIndexCreationTask<Product, Product_SearchCategoryName.Result>
    {
        public class Result
        {
            public string Name { get; set; }
            public string CategoryName { get; set; }
            public string Supplier { get; set; }
            public decimal PricePerUnit { get; set; }
        }

        public Product_SearchCategoryName()
        {
            Map = products => from p in products
                              let category = LoadDocument<Category>(p.Category)
                              select new Result
                              {
                                  Name = p.Name,
                                  CategoryName = category.Name,
                                  Supplier = p.Supplier,
                                  PricePerUnit = p.PricePerUnit
                              };
            
            Store(x => x.CategoryName, FieldStorage.Yes);
        }
    }
}