using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Sample.Worker.Persistence.Encryption.Models
{
    public class KeyVault
    {
        private Guid _id;
        private IReadOnlyList<string> _keyAltNames;
        private byte[] _keyMaterial;
        private DateTime _creationDate;
        private DateTime _updateDate;
        private int _status;
        private object _masterKey;

        [BsonId]
        public Guid Id
        {
            get => _id;
            set => _id = value;
        }

        public IReadOnlyList<string> KeyAltNames
        {
            get => _keyAltNames;
            set => _keyAltNames = value;
        }

        public byte[] KeyMaterial
        {
            get => _keyMaterial;
            set => _keyMaterial = value;
        }

        public DateTime CreationDate
        {
            get => _creationDate;
            set => _creationDate = value;
        }

        public DateTime UpdateDate
        {
            get => _updateDate;
            set => _updateDate = value;
        }

        public int Status
        {
            get => _status;
            set => _status = value;
        }

        public object MasterKey
        {
            get => _masterKey;
            set => _masterKey = value;
        }
    }
}