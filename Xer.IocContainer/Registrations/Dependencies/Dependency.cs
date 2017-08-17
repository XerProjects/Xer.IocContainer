using System;
using System.Collections.Generic;
using System.Text;

namespace Xer.IocContainer.Registrations.Dependencies
{
    public abstract class Dependency
    {
        public IRegistration Registration { get; private set; }

        public abstract DependencyType DependencyType { get; }

        public Dependency(IRegistration registrationOfThisDependency)
        {
            Registration = registrationOfThisDependency;
        }
    }
}
