using System;
using MongoDB.Driver;
using Sample.Worker.Persistence.Encryption.Models;

namespace Sample.Worker.Persistence.Encryption.Helpers
{
    public interface IMongoDBHelper
    {
        string DatabaseName { get; }
        
        IMongoClient GetMongoDBClientWithAutoEncrypting(KmsProvider kmsProvider);
    }

    public class MongoDBHelper : IMongoDBHelper
    {
        private readonly string _databaseName;
        private readonly string _mongoDBConnectionString;
        private readonly string _mongoDBCustomerMasterKeyBase64;
        private readonly CollectionNamespace _keyVaultNamespace = CollectionNamespace.FromFullName("encryption.__keyVault");
        
        public string DatabaseName => _databaseName;

        public MongoDBHelper()
        {
            _databaseName = "myDatabase";
            _mongoDBCustomerMasterKeyBase64 = Environment.GetEnvironmentVariable("CMK_BASE64");
            _mongoDBConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
        }
        
        public IMongoClient GetMongoDBClientWithAutoEncrypting(KmsProvider kmsProvider)
        {
            var dekHelper = new DekHelper(_mongoDBConnectionString, _keyVaultNamespace, _mongoDBCustomerMasterKeyBase64);
            var dataEncryptionKeyBase64 = dekHelper.GetUsingLocalProvider();
            
            var autoEncryptHelper = new AutoEncryptHelper(_mongoDBConnectionString, DatabaseName, _keyVaultNamespace, _mongoDBCustomerMasterKeyBase64);
            var mongoClientComAutoEncrypting = autoEncryptHelper.CreateMongoDBClientWithAutoEncryption(dataEncryptionKeyBase64, kmsProvider);

            return mongoClientComAutoEncrypting;
        }
    }
}