using System;
using Humanizer;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;
using System.Threading;
using MongoDB.Driver.Encryption;
using Sample.Worker.Persistence.Encryption.Models;

namespace Sample.Worker.Persistence.Encryption.Helpers
{
    public class DekHelper
    {
        private const string AlternativeNameDataEncryptionKey = "MY_ALTERNATIVE_NAME";
        
        private readonly string _mongoDBCustomerMasterKeyBase64;
        
        private readonly IMongoClient _keyVaultMongoClient;
        private readonly CollectionNamespace _keyVaultNamespace;
        private readonly IMongoCollection<KeyVault> _keyVaultCollection;
        
        public DekHelper(string connectionString, CollectionNamespace keyVaultNamespace, string mongoDBCustomerMasterKeyBase64)
        {
            _keyVaultNamespace = keyVaultNamespace;
            _keyVaultMongoClient = new MongoClient(connectionString);
            _mongoDBCustomerMasterKeyBase64 = mongoDBCustomerMasterKeyBase64;
            
            _keyVaultCollection = _keyVaultMongoClient
                .GetDatabase(_keyVaultNamespace.DatabaseNamespace.DatabaseName)
                .GetCollection<KeyVault>(_keyVaultNamespace.CollectionName);
        }
        
        public string GetUsingLocalProvider()
        {
            var localCustomerMasterKeyBytes = Convert.FromBase64String(_mongoDBCustomerMasterKeyBase64);
            
            var kmsProviderLocal = KmsProviderHelper.ObterLocal(localCustomerMasterKeyBytes);
            
            var clientEncryptionOptions = new ClientEncryptionOptions
            (
                kmsProviders: kmsProviderLocal,
                keyVaultClient: _keyVaultMongoClient,
                keyVaultNamespace: _keyVaultNamespace
            );
            
            Guid dataEncryptionKey;
            
            var DEKCheckResult = CheckExistenceDataEncryptionKeyWithAlternativeName();
            
            if (DEKCheckResult.Exists)
            {
                dataEncryptionKey = DEKCheckResult.DataEncryptionKey;
            }
            else
            {
                CreateUniqueIndexInKeyAltNamesFieldOfKeyVaultCollection();
                
                var clientEncryption =  new ClientEncryption(clientEncryptionOptions);
                var alternateKeyNames = new[] { AlternativeNameDataEncryptionKey };
                var dataKeyOptions = new DataKeyOptions(alternateKeyNames: alternateKeyNames);
                
                dataEncryptionKey = clientEncryption.CreateDataKey(KmsProvider.Local.Humanize(), dataKeyOptions, CancellationToken.None);
                
                clientEncryption.Dispose();
            }
            
            return Convert.ToBase64String(GuidConverter.ToBytes(dataEncryptionKey, GuidRepresentation.Standard));
        }
        
        private (bool Exists, Guid DataEncryptionKey) CheckExistenceDataEncryptionKeyWithAlternativeName()
        {
            var keyVault = _keyVaultCollection
                .Find(f => f.KeyAltNames.Contains(AlternativeNameDataEncryptionKey))
                .FirstOrDefault();

            return keyVault is not null ? (true, keyVault.Id) : (false, Guid.Empty);
        }
        
        private void CreateUniqueIndexInKeyAltNamesFieldOfKeyVaultCollection()
        {
            var keyAltNamesFieldNameInCamelCase = JsonNamingPolicy.CamelCase.ConvertName(nameof(KeyVault.KeyAltNames));
            
            var indexOptions = new CreateIndexOptions<KeyVault>
            {
                Unique = true,
                PartialFilterExpression = new BsonDocument
                {
                    { keyAltNamesFieldNameInCamelCase, new BsonDocument { { "$exists", new BsonBoolean(true) } } }
                }
            };
            
            var builder = Builders<KeyVault>.IndexKeys;
            var indexKeysDocument = builder.Ascending(keyAltNamesFieldNameInCamelCase);
            var indexModel = new CreateIndexModel<KeyVault>(indexKeysDocument, indexOptions);
            
            _keyVaultCollection.Indexes.CreateOne(indexModel);
        }
    }
}