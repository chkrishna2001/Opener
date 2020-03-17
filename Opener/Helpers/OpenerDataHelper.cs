using Opener.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Opener.Helpers
{
    public class OpenerDataHelper
    {
        private readonly string DataFilesLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataFiles");
        private const string KeyFileName = "OpenerKeys.xml";
        private readonly string KeyFilePath = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataFiles"), KeyFileName);
        private List<OKey> oKeys = new List<OKey>();
        private readonly XMLSerializerNDeSerializer xmlSerializerNDeSerializer;
        public OpenerDataHelper(XMLSerializerNDeSerializer xmlSerializerNDeSerializer)
        {
            this.xmlSerializerNDeSerializer = xmlSerializerNDeSerializer;
            if (File.Exists(KeyFilePath))
            {
                using (StreamReader sr = new StreamReader(KeyFilePath))
                {
                    oKeys = xmlSerializerNDeSerializer.DeSerialize<List<OKey>>(sr.ReadToEnd());
                }
            }
            else
            {
                if (!Directory.Exists(DataFilesLocation))
                {
                    Directory.CreateDirectory(DataFilesLocation);
                }
            }
            AddDefaultKeysAndSave();
        }
        public OKey GetKey(string key)
        {
            var currentKey = oKeys.FirstOrDefault(m => string.Equals(m.Key, key, StringComparison.OrdinalIgnoreCase));
            return currentKey;
        }
        private void AddDefaultKeysAndSave()
        {
            var defaultKeys = new List<OKey>() {
                new OKey() { Key = "g", KeyType = OKeyType.WebPath, Path = "https://www.google.com/search?q={0}" }
            };
            var keysTobeAdded = defaultKeys.Except(oKeys, new OKeyComparer());
            oKeys.AddRange(keysTobeAdded);
            var data = xmlSerializerNDeSerializer.Serialize(oKeys);
            File.WriteAllText(KeyFilePath, data);
        }
    }
}
