﻿namespace imgbruh.Infrastructure
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using System.Configuration;
    using System.IO;
    using System.Threading.Tasks;

    public class FileStorage
    {
        private readonly string ResourceName = ConfigurationManager.AppSettings["imgbruh.blob.core:resourceName"];
        //private const string ResourceName = "imgbruh";
        private const string ProtocolPrefix = "https://";
        private readonly string EndpointSuffix = ConfigurationManager.AppSettings["imgbruh.blob.core:endpointSuffix"];
        private readonly string Key1 = ConfigurationManager.AppSettings["imgbruh.blob.core:key1"];
        private readonly string Key2 = ConfigurationManager.AppSettings["imgbruh.blob.core:key2"];
        private readonly string DefaultContainer = ConfigurationManager.AppSettings["imgbruh.blob.core:defaultContainer"];

        public async Task<string> UploadBlobAsync(Stream fileStream, string name)
        {
#if DEBUG
            var localStorageConnectionString = ConfigurationManager.AppSettings["localStorageConnection"];
            var storageAccount = CloudStorageAccount.Parse(localStorageConnectionString);
            var url = "http://127.0.0.1:10000/devstoreaccount1/default/" + name;
#else
            var storageCredentials = new StorageCredentials(ResourceName, Key1);
            var storageAccount = new CloudStorageAccount(storageCredentials, null, true);
            var url = ProtocolPrefix + ResourceName + EndpointSuffix + "/" + DefaultContainer + "/"+ name;
#endif
            var storageClient = storageAccount.CreateCloudBlobClient();
            var defaultContainer = storageClient.GetContainerReference(DefaultContainer);
            var blob = defaultContainer.GetBlockBlobReference(name);
            using (fileStream)
            {
                await blob.UploadFromStreamAsync(fileStream);
            }
            return url;
        }
    }    
}