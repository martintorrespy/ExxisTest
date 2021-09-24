using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Afiliados.Utils
{
    public class CacheManager
    {
        public ICacheProvider Cache { get; set; }
        public CacheManager()
            : this(new DefaultCacheProvider())
        {
        }
        public CacheManager(ICacheProvider cacheProvider)
        {
            this.Cache = cacheProvider;
        }
    }
}