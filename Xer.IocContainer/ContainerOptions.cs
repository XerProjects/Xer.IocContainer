using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xer.IocContainer.Configuration.ConstructorSelectors;
using Xer.IocContainer.Configuration.PropertySelectors;
using Xer.IocContainer.Registrations.Dependencies;

namespace Xer.IocContainer
{
    public class ContainerOptions
    {
        public bool ThrowExceptionOnDuplicateRegistration { get; set; } = false;
        public bool AllowManualDependencyResolvers { get; set; } = true;

        public ConstructorSelector ConstructorSelector { get; set; } = new FirstDeclaredConstructorSelector();
        public InjectablePropertiesSelector InjectablePropertiesSelector { get; set; } = new PropertiesMarkedWithInjectAttributeSelector();

        //public IDependencySelector<ConstructorDependency> ConstructorDependencySelector { get; set; } = new ConstructorDependencySelector();

        //public IDependencySelector<PropertyDependency> PropertyDependencySelector { get; set;  } = new InjectPropertyDependencySelector();
    }
}
