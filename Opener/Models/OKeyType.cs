using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opener.Models
{
    public class OKeyType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public enum KeyTypeId
    {
        WebPath = 1,
        LocalPath = 2,
        JsonData = 3,
        Data = 4,
        SecureData = 5
    }
}
