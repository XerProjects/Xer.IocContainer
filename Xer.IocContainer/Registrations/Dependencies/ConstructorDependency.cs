using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Xer.IocContainer.Registrations.Dependencies
{
    public class ConstructorDependency : Dependency
    {
        public ConstructorInfo Constructor { get; private set; }

        public ConstructorDependency(ConstructorInfo constructor, IRegistration registrationOfThisConstructorDependency) 
            : base(registrationOfThisConstructorDependency)
        {
            Constructor = constructor;
        }

        public override DependencyType DependencyType => DependencyType.Constructor;
    }
}
