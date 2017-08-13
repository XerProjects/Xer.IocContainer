using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xer.IocContainer.Collections
{
    internal class SingletonInstanceCollection : KeyValueCollectionBase<Type, object>
    {
        protected override IEqualityComparer<Type> KeyEqualityComparer => TypeEqualityComparer.Instance;
    }
}
