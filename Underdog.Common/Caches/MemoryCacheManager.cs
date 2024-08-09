using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Underdog.Common.Caches.Extensions;

namespace Underdog.Common.Caches
{
    public class MemoryCacheManager : IMemoryCache
    {
        private readonly IOptions<MemoryCacheOptions> _optionsAccessor;

        private IMemoryCache _inner;

        public MemoryCacheManager(IOptions<MemoryCacheOptions> optionsAccessor)
        {
            _optionsAccessor = optionsAccessor;
            _inner = new MemoryCache(_optionsAccessor);
        }

        public void Dispose() => _inner.Dispose();

        public ICacheEntry CreateEntry(object key) => _inner.CreateEntry(key);

        public void Remove(object key) => _inner.Remove(key);

        public bool TryGetValue(object key, out object value) => _inner.TryGetValue(key, out value);

        public void Reset()
        {
            lock (_optionsAccessor)
            {
                var old = _inner;
                _inner = new MemoryCache(_optionsAccessor);
                old.Dispose();
            }
        }

        public IEnumerable<string> GetAllKeys()
        {
            return _inner.GetKeys<string>();
        }
    }
}
