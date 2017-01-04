namespace imgbruh.Infrastructure
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using System.Configuration;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web;

    public class FileStorage
    {
        private const string ResourceName = "imgbruh";
        private const string ProtocolPrefix = "https://";
        private readonly string EndpointSuffix = ConfigurationManager.AppSettings["imgbruh.blob.core:endpointSuffix"];
        private readonly string Key1 = ConfigurationManager.AppSettings["imgbruh.blob.core:key1"];
        private readonly string Key2 = ConfigurationManager.AppSettings["imgbruh.blob.core:key2"];
        private readonly string DefaultContainer = ConfigurationManager.AppSettings["imgbruh.blob.core:defaultContainer"];

        public async Task<string> UploadBlobAsync(Stream fileStream, string name)
        {
            var storageCredentials = new StorageCredentials(ResourceName, Key1);
            var storageAccount = new CloudStorageAccount(storageCredentials, EndpointSuffix, true);
            var storageClient = storageAccount.CreateCloudBlobClient();
            var defaultContainer = storageClient.GetContainerReference(DefaultContainer);
            var blob = await defaultContainer.GetBlobReferenceFromServerAsync(name);
            using (var stream = fileStream)
            {
                await blob.UploadFromStreamAsync(stream);
            }
            var url = ProtocolPrefix + ResourceName + EndpointSuffix;
            return url;
        }
    }    
}