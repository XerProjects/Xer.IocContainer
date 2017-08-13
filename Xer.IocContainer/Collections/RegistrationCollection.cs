using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xer.IocContainer.Exceptions;
using Xer.IocContainer.Registrations;

namespace Xer.IocContainer.Collections
{
    internal class RegistrationCollection : KeyValueCollectionBase<Type, IRegistration>
    {
        protected override IEqualityComparer<Type> KeyEqualityComparer => TypeEqualityComparer.Instance;

        public RegistrationCollection()
        {
        }

        public RegistrationCollection(IDictionary<Type, IRegistration> registrations)
            : base(registrations)
        {
        }
    }
}
