using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKR_Redis.Common
{
    public class KeysData
    {
        public List<KeyData> Keys { get; set; }
    }

    public class KeyData
    {
        public string Key;
        public string Value;
    }
}
