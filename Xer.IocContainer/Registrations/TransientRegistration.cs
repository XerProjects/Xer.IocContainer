using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xer.IocContainer.Registrations
{
    internal class TransientRegistration : RegistrationBase
    {
        public override RegistrationType RegistrationType => RegistrationType.Transient;

        public TransientRegistration(XerContainer container, Type interfaceType, Type implementationType)
            : base(container, interfaceType, implementationType)
        {
        }

    }
}
