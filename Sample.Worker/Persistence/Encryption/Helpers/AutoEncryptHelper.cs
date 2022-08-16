using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Sample.Worker.Models;
using MongoDB.Driver.Encryption;
using System.Collections.Generic;
using Sample.Worker.Persistence.Encryption.Models;

namespace Sample.Worker.Persistence.Encryption.Helpers
{
    public class AutoEncryptHelper
    {
        private readonly string _connectionString;
        private readonly string _mongoDBCustomerMasterKeyBase64;
        
        private readonly CollectionNamespace _fooNamespace;
        private readonly CollectionNamespace _keyVaultNamespace;
        
        public AutoEncryptHelper(string connectionString, string databaseName, CollectionNamespace keyVaultNamespace, string mongoDBCustomerMasterKeyBase64)
        {
            _connectionString = connectionString;
            _keyVaultNamespace = keyVaultNamespace;
            _mongoDBCustomerMasterKeyBase64 = mongoDBCustomerMasterKeyBase64;
            _fooNamespace = CollectionNamespace.FromFullName($"{databaseName}.{nameof(Foo).ToLower()}");
        }
        
        public IMongoClient CreateMongoDBClientWithAutoEncryption(string dataEncriptionKeyBase64, KmsProvider kmsProvider)
        {
            Dictionary<string, IReadOnlyDictionary<string, object>> kmsProviderLocal = new();
            
            if (kmsProvider == KmsProvider.Local)
            {
                var localCustomerMasterKeyBytes = Convert.FromBase64String(_mongoDBCustomerMasterKeyBase64);
                kmsProviderLocal = KmsProviderHelper.ObterLocal(localCustomerMasterKeyBytes);
            }
            
            var jsonSchemaFoo = JsonSchemaHelper.CreateJsonSchemaFoo(dataEncriptionKeyBase64);
            var schemaMap = new Dictionary<string, BsonDocument> { { _fooNamespace.ToString(), jsonSchemaFoo } };
            
            var clientSettings = MongoClientSettings.FromConnectionString(_connectionString);
            
            clientSettings.AutoEncryptionOptions = new AutoEncryptionOptions
            (
                schemaMap: schemaMap,
                kmsProviders: kmsProviderLocal,
                keyVaultNamespace: _keyVaultNamespace
            );

            return new MongoClient(clientSettings);
        }
    }
}