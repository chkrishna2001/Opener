using Opener.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Opener.Helpers
{
    public class OpenerDataHelper
    {
#if DEBUG
        private const string configuration = "debug";
#else
        private const string configuration = "release";
#endif

        private readonly string DataFilesLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"Opener", configuration, "DataFiles");
        private const string KeyFileName = "OpenerKeys.xml";
        private readonly string KeyFilePath = string.Empty;
        private AllData data = new AllData() { Keys= new List<OKey>(), KeyTypes = new List<OKeyType>()  };
        private readonly XMLSerializerNDeSerializer xmlSerializerNDeSerializer;
        public OpenerDataHelper(XMLSerializerNDeSerializer xmlSerializerNDeSerializer)
        {
            KeyFilePath = Path.Combine(DataFilesLocation, KeyFileName);
            this.xmlSerializerNDeSerializer = xmlSerializerNDeSerializer;
            if (File.Exists(KeyFilePath))
            {
                using (StreamReader sr = new StreamReader(KeyFilePath))
                {
                    data = xmlSerializerNDeSerializer.DeSerialize<AllData>(sr.ReadToEnd());
                }
            }
            else
            {
                if (!Directory.Exists(DataFilesLocation))
                {
                    Directory.CreateDirectory(DataFilesLocation);
                }
            }
            AddDefaultKeysAndKeyType();
        }
        public OKey GetKey(string key)
        {
            var currentKey = data.Keys.FirstOrDefault(m => string.Equals(m.Key, key, StringComparison.OrdinalIgnoreCase));
            return currentKey;
        }
        public OKey GetKeyById(string Id)
        {
            var currentKey = data.Keys.FirstOrDefault(m => string.Equals(m.Id, Id));
            return currentKey;
        }
        public List<OKeyType> GetKeyTypes()
        {
            return data.KeyTypes;
        }
        public List<OKey> GetKeys()
        {
            return data.Keys;
        }

        private void AddDefaultKeysAndKeyType()
        {
            data.KeyTypes = new List<OKeyType>()
            {
                KeyTypeIdToKeyType(KeyTypeId.WebPath),
                KeyTypeIdToKeyType(KeyTypeId.LocalPath),
                KeyTypeIdToKeyType(KeyTypeId.JsonData),
                KeyTypeIdToKeyType(KeyTypeId.Data),
                KeyTypeIdToKeyType(KeyTypeId.SecureData),
            };
            var defaultKeys = new List<OKey>() {
                new OKey() { Key = "g", KeyType = KeyTypeIdToKeyType(KeyTypeId.WebPath), Path = "https://www.google.com/search?q={0}" }
            };
            var keysTobeAdded = defaultKeys.Except(data.Keys, new OKeyComparer());
            data.Keys.AddRange(keysTobeAdded);
            SaveData();
        }
        public OKeyType KeyTypeIdToKeyType(KeyTypeId keyTypeId)
        {
            return new OKeyType() { Id = (int)keyTypeId, Name = keyTypeId.ToString() };
        }
        public void SaveAllKeys(IEnumerable<OKey> oKeys)
        {
            data.Keys = oKeys.ToList();
            SaveData();
        }
        public void SaveKey(OKey oKey)
        {
            if(oKey.KeyType.Id == (int)KeyTypeId.SecureData)
            {
                oKey.Path = oKey.Path.Base64Encode();
            }
            var existing = GetKeyById(oKey.Id);
            if(existing == null)
            {
                data.Keys.Add(oKey);
            }
            else
            {
                existing.Key = oKey.Key;
                existing.KeyType = oKey.KeyType;
                existing.Path = oKey.Path;
                data.Keys.Remove(existing);
                data.Keys.Add(existing);
            }
            SaveData();
        }
        public void DeletyeKey(OKey oKey)
        {
            data.Keys.Remove(oKey);
            SaveData();
        }
        private void SaveData()
        {
            var dataJson = xmlSerializerNDeSerializer.Serialize(data);
            File.WriteAllText(KeyFilePath, dataJson);
        }
    }
}
