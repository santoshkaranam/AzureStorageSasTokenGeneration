using Microsoft.WindowsAzure.Storage;
using System;

namespace Azure.Blob.SASBasedAccess.Interfaces
{
    public interface IStorageSasConfiguration
    {
        string ConnectionString { get; set; }

        SharedAccessAccountPermissions Permissions { get; set; } 

        DateTimeOffset ExpiryTime { get; set; } 
    }
}