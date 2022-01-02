using System.Linq;
using Raven.Client.Documents.Indexes;
using RavenDB.Tryout.App.Models;

namespace RavenDB.Tryout.App.Indexes
{
    public class Product_Search : AbstractIndexCreationTask<Product, Product_Search.Result>
    {
        public class Result
        {
            public string Name { get; set; }
            public string Category { get; set; }
            public string Supplier { get; set; }
            public decimal PricePerUnit { get; set; }
        }

        public Product_Search()
        {
            Map = products => from p in products
                              select new Result
                              {
                                  Name = p.Name,
                                  Category = p.Category,
                                  Supplier = p.Supplier,
                                  PricePerUnit = p.PricePerUnit
                              };
        }
    }
}