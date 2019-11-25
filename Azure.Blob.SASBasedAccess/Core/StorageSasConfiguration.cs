using System;
using Azure.Blob.SASBasedAccess.Interfaces;
using Microsoft.WindowsAzure.Storage;

namespace Azure.Blob.SASBasedAccess.Core
{
    public class StorageSasConfiguration : IStorageSasConfiguration
    {
        public string ConnectionString { get; set; }

        public SharedAccessAccountPermissions Permissions { get; set; } = SharedAccessAccountPermissions.Read | SharedAccessAccountPermissions.List;

        public DateTimeOffset ExpiryTime { get; set; } = DateTimeOffset.Now.AddMinutes(5);
    }
}   
