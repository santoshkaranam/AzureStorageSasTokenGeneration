using Azure.Blob.SASBasedAccess.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;

namespace TestAzureSas
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=azureblobsastest1;AccountKey=foexxYwYyzN3g+gDwr72oKX9eQy9XYCh9rg4j+NPcZnfwfKvggx/PN0k38pWFXJMC2MEmBwggUfPjaviPhNVwQ==;EndpointSuffix=core.windows.net";
            AzureResourceSasGenerator sasGenerator = new AzureResourceSasGenerator()
                .With(connectionString)
                .With(SharedAccessAccountPermissions.List | SharedAccessAccountPermissions.Read);

            var token = sasGenerator.GetStorageSasToken();
            UriBuilder sasUri = new UriBuilder()
            {
                Scheme = "https",
                Host = string.Format("{0}.blob.core.windows.net", "azureblobsastest1"),
                Query = token
            };
            var blobClient = new CloudBlobClient(sasUri.Uri);
            var containerRef = blobClient.GetContainerReference("app1container");
            var blobRef = containerRef.GetBlockBlobReference("blob.txt");
            var content = blobRef.DownloadTextAsync().Result;
            Console.WriteLine(content);

            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid()+ ".txt");
            blobRef.DownloadToFileAsync(file, FileMode.Create).Wait();
            Console.Read();
        }
    }
}
