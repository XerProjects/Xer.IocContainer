using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xer.IocContainer.Collections
{
    internal class InstanceCollection<TKey> : KeyValueCollectionBase<TKey, object>
    {
        protected override IEqualityComparer<TKey> KeyEqualityComparer => ReferenceEqualityComparer<TKey>.Instance;
    }
}
