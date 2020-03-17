using System.Collections.Generic;

namespace Opener.Models
{
    public class OKey
    {
        public string Key { get; set; }
        public OKeyType KeyType { get; set; }
        public string Path { get; set; }
    }
    public enum OKeyType
    {
        WebPath,
        LocalPath,
        JsonData,
        Data
    }
    public class OKeyComparer : IEqualityComparer<OKey>
    {
        public bool Equals(OKey x, OKey y)
        {
            return string.Equals(x.Key, y.Key, System.StringComparison.OrdinalIgnoreCase) && x.KeyType == y.KeyType;
        }

        public int GetHashCode(OKey obj)
        {
            return $"{obj.Key}-{obj.KeyType.ToString()}".GetHashCode();
        }
    }
}
