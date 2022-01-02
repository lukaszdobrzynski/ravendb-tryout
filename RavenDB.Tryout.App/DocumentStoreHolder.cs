using System;
using Raven.Client.Documents;
using RavenDB.Tryout.App.Indexes;

namespace RavenDB.Tryout.App
{
    public class DocumentStoreHolder
    {
        private static readonly Lazy<IDocumentStore> LazyStore =
            new Lazy<IDocumentStore>(() =>
            {
                var store = new DocumentStore
                {
                    Urls = new[] { "http://localhost:8080" },
                    Database = "CRUD"
                };

                store.Initialize();
                
                store.ExecuteIndex(new Product_SearchCategoryName());
                store.ExecuteIndex(new Orders_ProductCont());

                return store;
            });

        public static IDocumentStore Store => LazyStore.Value;
    }
}