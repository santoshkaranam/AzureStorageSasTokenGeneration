using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;

namespace Azure.Blob.SASBasedAccess.Interfaces
{
    public class AzureResourceSasGenerator : IAzureStorageSasTokenGenerator
    {
        public AzureResourceSasGenerator(IStorageSasConfiguration blobConfiguration)
        {
            BlobConfiguration = blobConfiguration;
            ConnectionString = blobConfiguration.ConnectionString;
            ExpiryDateTimeOffset = blobConfiguration.ExpiryTime;
            Permissions = blobConfiguration.Permissions;
        }

        public AzureResourceSasGenerator()
        {
            Permissions = SharedAccessAccountPermissions.Read | SharedAccessAccountPermissions.List;

            ExpiryDateTimeOffset = DateTimeOffset.Now.AddMinutes(5);
        }

        public string ConnectionString { get; set; }

        public SharedAccessAccountPermissions Permissions { get; set; } = SharedAccessAccountPermissions.Read | SharedAccessAccountPermissions.List;

        public DateTimeOffset ExpiryDateTimeOffset { get; set; } = DateTimeOffset.Now.AddMinutes(5);

        public IStorageSasConfiguration BlobConfiguration { get; }

        public AzureResourceSasGenerator With(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception();
            }
            ConnectionString = connectionString;
            return this;
        }

        public AzureResourceSasGenerator With(SharedAccessAccountPermissions permissions)
        {
            if (permissions == default)
            {
                throw new Exception();
            }
            Permissions = permissions;
            return this;
        }

        public AzureResourceSasGenerator With(DateTimeOffset expiryTime)
        {
            if (expiryTime == null)
            {
                throw new Exception();
            }
            ExpiryDateTimeOffset = expiryTime;
            return this;
        }
        public string GetStorageSasToken()
        {
            CloudStorageAccount storageAccount = InitializeStorageAccount();

            SharedAccessAccountPolicy policy = GetPolicy();
            string sasToken = storageAccount.GetSharedAccessSignature(policy);
            return sasToken;
        }

        private SharedAccessAccountPolicy GetPolicy()
        {
            return new SharedAccessAccountPolicy()
            {
                Permissions = Permissions,
                Services = SharedAccessAccountServices.Blob,
                ResourceTypes = SharedAccessAccountResourceTypes.Container | SharedAccessAccountResourceTypes.Object,
                SharedAccessExpiryTime = ExpiryDateTimeOffset,
                Protocols = SharedAccessProtocol.HttpsOnly,
            };
        }

        public string GetBlobContainerSasToken(string accountName, string containerName, string permissions, DateTimeOffset expiryDateTimeOffset)
        {
            var accessToken = GetStorageSasToken();
            UriBuilder sasUri = new UriBuilder()
            {
                Scheme = "https",
                Host = string.Format("{0}.blob.core.windows.net", accountName),
                Query = accessToken
            };
            var blobClient = new CloudBlobClient(sasUri.Uri);
            var containerRef = blobClient.GetContainerReference(containerName);
            var permissionsEnum = SharedAccessBlobPolicy.PermissionsFromString(permissions);
            var token = containerRef.GetSharedAccessSignature(new SharedAccessBlobPolicy
            {
                Permissions = permissionsEnum,
                SharedAccessExpiryTime = expiryDateTimeOffset,
            });
            return token;
        }

        private CloudStorageAccount InitializeStorageAccount()
        {
            if (ConnectionString == null)
            {
                throw new Exception("Connection String cannot be null. Initialize before use");
            }
            var storageAccount = CloudStorageAccount.Parse(ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient(); // to check if connectionstring is proper.
            return storageAccount;
        }

        public string GenerateSasToken(SharedAccessAccountPolicy sharedAccessAccountPolicy)
        {
            var storageAccount = InitializeStorageAccount();
            string sasToken = storageAccount.GetSharedAccessSignature(sharedAccessAccountPolicy);
            return sasToken;
        }
    }
}