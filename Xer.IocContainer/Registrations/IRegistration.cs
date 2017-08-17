using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xer.IocContainer.InstanceFactories;
using Xer.IocContainer.LifetimeScopes;
using Xer.IocContainer.Registrations.Dependencies;

namespace Xer.IocContainer.Registrations
{
    public interface IRegistration : ICompileable, IDisposable
    {
        /// <summary>
        /// Selected constructor of this registered object.
        /// </summary>
        ConstructorInfo Constructor { get; }

        /// <summary>
        /// Properties of this registered object that are selected for injection.
        /// </summary>
        IReadOnlyList<PropertyInfo> InjectableProperties { get; }

        /// <summary>
        /// Registered type of this registered object.
        /// </summary>
        Type RegisteredType { get; }

        /// <summary>
        /// Contract's Type Info.
        /// </summary>
        TypeInfo RegisteredTypeInfo { get; }

        /// <summary>
        /// Implementation type of this registered object.
        /// </summary>
        Type ImplementationType { get; }

        /// <summary>
        /// Implementation's Type Info.
        /// </summary>
        TypeInfo ImplementationTypeInfo { get; }

        /// <summary>
        /// Registration's constructor dependencies as determined by the configured IDependencySelector<ConstructorDependency>
        /// </summary>
        IReadOnlyList<ConstructorDependency> ConstructorDependencies { get; }

        /// <summary>
        /// Registration's property dependencies as determined by the configured IDependencySelector<PropertyDependency>
        /// </summary>
        IReadOnlyList<PropertyDependency> PropertyDependencies { get; }

        /// <summary>
        /// Creates an instance of this registration.
        /// </summary>
        IInstanceFactory InstanceFactory { get; }

        /// <summary>
        /// Registration type.
        /// </summary>
        InstanceLifetime InstanceLifetime { get; }

        /// <summary>
        /// Manages lifetime of this registration's instances.
        /// </summary>
        //LifetimeScope LifetimeScope { get; }

        /// <summary>
        /// Owning container.
        /// </summary>
        XerContainer Container { get; }

        /// <summary>
        /// Get an instance of this registered object.
        /// </summary>
        object GetInstance();
        
        /// <summary>
        /// Get an instance of this registered object from scope, if available.
        /// </summary>
        //object GetInstance(LifetimeScope lifetimeScope);
    }
}
