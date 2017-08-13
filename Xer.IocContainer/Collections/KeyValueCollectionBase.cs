using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Xer.IocContainer.Collections
{
    internal abstract class KeyValueCollectionBase<TKey, TValue> : IKeyValueCollection<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _storage;

        protected abstract IEqualityComparer<TKey> KeyEqualityComparer { get; }

        public int Count => _storage.Count;

        public TValue this[TKey key] => GetValue(key);

        public KeyValueCollectionBase()
        {
            _storage = new Dictionary<TKey, TValue>(KeyEqualityComparer);
        }

        public KeyValueCollectionBase(IDictionary<TKey, TValue> collection)
        {
            _storage = collection;
        }

        public void Add(TKey key, TValue value)
        {
            _storage.Add(key, value);
        }

        public bool Contains(TKey key)
        {
            TValue value;

           return _storage.TryGetValue(key, out value);
        }

        protected TValue GetValue(TKey key)
        {
            TValue value = default(TValue);

            _storage.TryGetValue(key, out value);

            return value;
        }

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return _storage.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _storage.Values.GetEnumerator();
        }
    }
}
