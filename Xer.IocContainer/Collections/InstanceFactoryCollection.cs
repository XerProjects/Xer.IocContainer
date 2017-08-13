using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xer.IocContainer.InstanceFactories;

namespace Xer.IocContainer.Collections
{
    internal class InstanceFactoryCollection : KeyValueCollectionBase<Type, IInstanceFactory>
    {
        protected override IEqualityComparer<Type> KeyEqualityComparer => TypeEqualityComparer.Instance;

        public InstanceFactoryCollection()
        { 
        }

        public InstanceFactoryCollection(IDictionary<Type, IInstanceFactory> factories)
            : base(factories)
        {
        }
    }
}
