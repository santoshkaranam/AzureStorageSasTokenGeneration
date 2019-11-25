using Azure.Blob.SASBasedAccess.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Azure.Blob.SASBasedAccess.Core;

namespace Azure.Blob.SasTests
{
    [TestFixture]
    public class AzureResourceSasGeneratorTests
    {
        [OneTimeSetUp]
        public async Task SetDefaultData()
        {
            var blobConfig = GetBlobConfig();
            var blobSasGenerator = new AzureResourceSasGenerator(blobConfig).With(SharedAccessAccountPermissions.Write| SharedAccessAccountPermissions.Create);
            var accessToken = blobSasGenerator.GetStorageSasToken();
            CloudBlobClient blobClient = GetClient(accessToken);
            var containerRef = blobClient.GetContainerReference("app1container");
            await containerRef.CreateIfNotExistsAsync();
            var blobRef = containerRef.GetBlockBlobReference("blob1.txt");            
            await blobRef.UploadTextAsync("SomeText");
        }
        [Test]
        public void GenerateReadAccessSasToken_ValidToken_ReadSuccessful()
        {
            //arrrange
            var blobConfig = GetBlobConfig();
            var blobSasGenerator = new AzureResourceSasGenerator(blobConfig);
            var accessToken = blobSasGenerator.GetStorageSasToken();
            CloudBlobClient blobClient = GetClient(accessToken);
            var containerRef = blobClient.GetContainerReference("app1container");
            var blobRef = containerRef.GetBlockBlobReference("blob.txt");


            //act
            var content = blobRef.DownloadTextAsync().Result;

            //assert
            Assert.IsNotNull(content);
        }

        private static CloudBlobClient GetClient(string accessToken)
        {
            UriBuilder sasUri = new UriBuilder()
            {
                Scheme = "https",
                Host = string.Format("{0}.blob.core.windows.net", "azureblobsastest1"),
                Query = accessToken
            };
            var blobClient = new CloudBlobClient(sasUri.Uri);
            return blobClient;
        }

        public StorageSasConfiguration GetBlobConfig()
        {
            return new StorageSasConfiguration()
            {
                ConnectionString = "DefaultEndpointsProtocol=https;AccountName=azureblobsastest1;AccountKey=foexxYwYyzN3g+gDwr72oKX9eQy9XYCh9rg4j+NPcZnfwfKvggx/PN0k38pWFXJMC2MEmBwggUfPjaviPhNVwQ==;EndpointSuffix=core.windows.net"
            };
        }
    }
}
