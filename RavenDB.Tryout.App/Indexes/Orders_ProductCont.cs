using System.Linq;
using Raven.Client.Documents.Indexes;
using RavenDB.Tryout.App.Models;

namespace RavenDB.Tryout.App.Indexes
{
    public class Orders_ProductCont : AbstractIndexCreationTask<Order, Orders_ProductCont.Result>
    {
        public class Result
        {
            public string Id { get; set; }
            public string Company { get; set; }
            public string Employee { get; set; }
            public bool AnyProductDiscontinued { get; set; }
        }

        public Orders_ProductCont()
        {
            Map = orders => from order in orders
                            let discontinuedList =
                                order.Lines.Select(x => LoadDocument<Product>(x.Product).Discontinued)
                                
                            let wasAnyDiscontinued = discontinuedList.Any(x => x == true)
                            
                            select new Result
                            {
                                Company = order.Company,
                                Employee = order.Employee,
                                AnyProductDiscontinued = wasAnyDiscontinued
                            };
        }
    }
}