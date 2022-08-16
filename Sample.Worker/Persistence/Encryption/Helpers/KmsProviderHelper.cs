using Humanizer;
using System.Collections.Generic;
using Sample.Worker.Persistence.Encryption.Models;

namespace Sample.Worker.Persistence.Encryption.Helpers
{
    public static class KmsProviderHelper
    {
        public static Dictionary<string, IReadOnlyDictionary<string, object>> ObterLocal(byte[] customerMasterKeyBytes)
        {
            var localOptions = new Dictionary<string, object> { { "key", customerMasterKeyBytes } };
            
            return new Dictionary<string, IReadOnlyDictionary<string, object>> { { KmsProvider.Local.Humanize(), localOptions } };
        }
    }
}