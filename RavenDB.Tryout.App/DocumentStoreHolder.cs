using System;
using Raven.Client.Documents;

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
                    Database = ""
                };

                return store.Initialize();
            });

        public static IDocumentStore Store => LazyStore.Value;
    }
}