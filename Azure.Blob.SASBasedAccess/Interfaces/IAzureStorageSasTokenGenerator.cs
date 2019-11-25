using System;
using Azure.Blob.SASBasedAccess.Core;
using Microsoft.WindowsAzure.Storage;

namespace Azure.Blob.SASBasedAccess.Interfaces
{
    public interface IAzureStorageSasTokenGenerator
    {
        IStorageSasConfiguration BlobConfiguration { get; }
        string ConnectionString { get; set; }
        DateTimeOffset ExpiryDateTimeOffset { get; set; }
        SharedAccessAccountPermissions Permissions { get; set; }

        string GenerateSasToken(SharedAccessAccountPolicy sharedAccessAccountPolicy);
        string GetBlobContainerSasToken(string accountName, string containerName, string permissions, DateTimeOffset expiryDateTimeOffset);
        string GetStorageSasToken();
        AzureResourceSasGenerator With(DateTimeOffset expiryTime);
        AzureResourceSasGenerator With(SharedAccessAccountPermissions permissions);
        AzureResourceSasGenerator With(string connectionString);
    }
}