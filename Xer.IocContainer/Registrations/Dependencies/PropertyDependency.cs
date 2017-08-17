using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Xer.IocContainer.Registrations.Dependencies
{
    public class PropertyDependency : Dependency
    {
        public PropertyInfo PropertyInfo { get; private set; }

        public PropertyDependency(PropertyInfo propertyInfo, IRegistration registrationOfThisPropertyDependency) 
            : base(registrationOfThisPropertyDependency)
        {
            PropertyInfo = propertyInfo;
        }

        public override DependencyType DependencyType => DependencyType.Property;
    }
}
