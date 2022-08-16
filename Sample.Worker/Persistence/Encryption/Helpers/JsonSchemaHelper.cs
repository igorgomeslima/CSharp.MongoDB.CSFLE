using System;
using MongoDB.Bson;
using System.Text.Json;
using Sample.Worker.Models;

namespace Sample.Worker.Persistence.Encryption.Helpers
{
    public static class JsonSchemaHelper
    {
        private static readonly string RANDOM_ENCRYPTION_TYPE = "AEAD_AES_256_CBC_HMAC_SHA_512-Random";
        private static readonly string DETERMINISTIC_ENCRYPTION_TYPE = "AEAD_AES_256_CBC_HMAC_SHA_512-Deterministic";
        
        private static BsonDocument CreateEncryptedMetadata(string dataEncriptionKeyBase64)
        {
            var keyId = new BsonBinaryData(Convert.FromBase64String(dataEncriptionKeyBase64), BsonBinarySubType.UuidStandard);
            
            return new BsonDocument(nameof(keyId), new BsonArray(new[] { keyId }));
        }
        
        private static BsonDocument CreateEncryptedField(string bsonType, bool isDeterministic)
        {
            return new BsonDocument
            {
                {
                    "encrypt",
                    new BsonDocument
                    {
                        { "bsonType", bsonType },
                        { "algorithm", isDeterministic ? DETERMINISTIC_ENCRYPTION_TYPE : RANDOM_ENCRYPTION_TYPE }
                    }
                }
            };
        }
        
        public static BsonDocument CreateJsonSchemaFoo(string dataEncriptionKeyBase64)
        {
            return new BsonDocument
            {
                { "bsonType", "object" },
                { "encryptMetadata", CreateEncryptedMetadata(dataEncriptionKeyBase64) },
                {
                    "properties",
                    new BsonDocument
                    {
                        {  JsonNamingPolicy.CamelCase.ConvertName(nameof(Foo.Name)), CreateEncryptedField(bsonType: "string", isDeterministic: true) },
                        {  JsonNamingPolicy.CamelCase.ConvertName(nameof(Foo.MotherName)), CreateEncryptedField(bsonType: "string", isDeterministic: true) },
                    }
                }
            };
        }
    }
}