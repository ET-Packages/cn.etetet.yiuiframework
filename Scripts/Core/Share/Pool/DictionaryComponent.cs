using System;
using System.Collections.Generic;

namespace ET
{
    public partial class DictionaryComponent<T, V> : Dictionary<T, V>, IPool
    {
        public DictionaryComponent()
        {
        }

        public static DictionaryComponent<T, V> Create()
        {
            return ObjectPool.Fetch(typeof(DictionaryComponent<T, V>)) as DictionaryComponent<T, V>;
        }

        public void Dispose()
        {
            ObjectPool.Recycle(this);
        }

        public bool IsFromPool { get; set; }
    }
}
