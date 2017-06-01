using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Configuration;
using imgbruh.Models;
using System.Threading.Tasks;

namespace imgbruh.Infrastructure
{
    

    public class DataClient : IDisposable
    {
        private string EndpointUrl = ConfigurationManager.AppSettings["cosmos:AccountName"];
        private string PrimaryKey = ConfigurationManager.AppSettings["cosmos:AccountKey"];
        private readonly DocumentClient client;

        public DataClient()
        {
            client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            var db = client.CreateDatabaseIfNotExistsAsync(new Database { Id = "imgbruh" });
            client.CreateDocumentCollectionIfNotExistsAsync(EndpointUrl, new DocumentCollection { Id = "pics", PartitionKey = new PartitionKeyDefinition { Paths = new System.Collections.ObjectModel.Collection<string> { "/codename" } } });
        }

        //public async Task CreateAsync(Img image)
        //{
        //    return client.CreateDocumentAsync(
        //        UriFactory.CreateDocumentCollectionUri(""
        //        )
        //}

        public void Dispose()
        {
            if (client != null)
                client.Dispose();
        }

    }
}