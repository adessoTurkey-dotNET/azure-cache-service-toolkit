using OKR_Redis.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKR_Redis.Interface
{
    public interface ICacheService
    {
        bool Exists(string key);
        Task Save(string key, string value);
        Task<string> Get(string key);
        Task<KeysData> GetAll();
        Task Delete(string key);
        Task Flush();
    }
}
