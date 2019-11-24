using Microsoft.WindowsAzure.Storage;
using System;

namespace Azure.Blob.SASBasedAccess.Interfaces
{
    public class BlobConfiguration : IStorageSasConfiguration
    {
        public string ConnectionString { get; set; }

        public SharedAccessAccountPermissions Permissions { get; set; } = SharedAccessAccountPermissions.Read | SharedAccessAccountPermissions.List;

        public DateTimeOffset ExpiryTime { get; set; } = DateTimeOffset.Now.AddMinutes(5);
    }
}   
