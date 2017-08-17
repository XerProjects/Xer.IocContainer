using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xer.IocContainer.LifetimeScopes;

namespace Xer.IocContainer.Registrations
{
    internal class TransientRegistration : RegistrationBase
    {
        public override InstanceLifetime InstanceLifetime => InstanceLifetime.Transient;
        //public override LifetimeScope LifetimeScope => new TransientLifetimeScope(Container);

        public TransientRegistration(XerContainer container, Type interfaceType, Type implementationType)
            : base(container, interfaceType, implementationType)
        {
        }
    }
}
