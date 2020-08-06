using System;
using System.Collections.Generic;

namespace Opener.Models
{
    public class OKey
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Key { get; set; }
        public OKeyType KeyType { get; set; }
        public string Path { get; set; }
    }
    public class OKeyComparer : IEqualityComparer<OKey>
    {
        public bool Equals(OKey x, OKey y)
        {
            return string.Equals(x.Key, y.Key, System.StringComparison.OrdinalIgnoreCase) && x.KeyType.Id == y.KeyType.Id;
        }

        public int GetHashCode(OKey obj)
        {
            return $"{obj.Key}-{obj.KeyType.Id.ToString()}".GetHashCode();
        }
    }
}
