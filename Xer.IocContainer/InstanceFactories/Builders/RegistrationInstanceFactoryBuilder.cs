using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xer.IocContainer.Registrations;

namespace Xer.IocContainer.InstanceFactories.Builders
{
    internal class RegistrationInstanceFactoryBuilder : IInstanceFactoryBuilder
    {
        private readonly IRegistration _registration;

        public RegistrationInstanceFactoryBuilder(IRegistration registration)
        {
            _registration = registration;
        }

        public IInstanceFactory BuildInstanceFactory()
        {
            return new RegistrationInstanceFactory(_registration);
        }
    }
}
