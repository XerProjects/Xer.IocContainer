using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xer.IocContainer.InstanceFactories;
using Xer.IocContainer.InstanceFactories.Builders;
using Xer.IocContainer.LifetimeScopes;
using Xer.IocContainer.Registrations.Dependencies;

namespace Xer.IocContainer.Registrations
{
    /// <summary>
    /// Registration decorator to support scoping.
    /// </summary>
    internal class ScopedRegistration : IRegistration
    {
        private readonly LifetimeScope _lifetimeScope;
        private readonly IRegistration _registration;

        public LifetimeScope LifetimeScope => _lifetimeScope;

        public IInstanceFactory InstanceFactory => _registration.InstanceFactory;

        public InstanceLifetime InstanceLifetime => InstanceLifetime.Scoped;

        public ConstructorInfo Constructor => _registration.Constructor;

        public IReadOnlyList<PropertyInfo> InjectableProperties => _registration.InjectableProperties;

        public Type RegisteredType => _registration.RegisteredType;

        public TypeInfo RegisteredTypeInfo => _registration.RegisteredTypeInfo;

        public Type ImplementationType => _registration.ImplementationType;

        public TypeInfo ImplementationTypeInfo => _registration.ImplementationTypeInfo;

        public IReadOnlyList<ConstructorDependency> ConstructorDependencies => _registration.ConstructorDependencies;

        public IReadOnlyList<PropertyDependency> PropertyDependencies => _registration.PropertyDependencies;

        public XerContainer Container => _registration.Container;

        public bool IsCompiled => _registration.IsCompiled;

        public ScopedRegistration(IRegistration registration, LifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
            _registration = registration;
        }

        public object GetInstance()
        {
            // Get from internal scope.
           return LifetimeScope.Resolve(_registration);
        }

        public void Compile()
        {
            if(!_registration.IsCompiled)
            {
                _registration.Compile();
            }
        }

        public void Dispose()
        {
            _registration.Dispose();
        }
    }
}
