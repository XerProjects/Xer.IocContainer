using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xer.IocContainer.Collections
{
    public interface IKeyValueCollection<TKey, TValue> : IEnumerable<TValue>
    {
        TValue this[TKey key] { get; }
        int Count { get; }

        bool TryGetValue(TKey key, out TValue value);
        void Add(TKey key, TValue value);
        bool Contains(TKey key);
        bool Remove(TKey key);
    }
}
