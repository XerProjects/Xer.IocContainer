using Xer.IocContainer.InstanceFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xer.IocContainer.Collections;
using Xer.IocContainer.InstanceFactories.Builders;
using Xer.IocContainer.Exceptions;

namespace Xer.IocContainer.Registrations
{
    internal abstract class RegistrationBase : IRegistration, ICompileable
    {
        private static readonly InstanceFactoryCollection _instanceFactories = new InstanceFactoryCollection();

        private Lazy<ConstructorInfo> _constructor;
        private Lazy<List<Type>> _constructorArgumentTypes;
        private Lazy<List<IRegistration>> _dependencyRegistrations;

        public abstract RegistrationType RegistrationType { get; }

        protected XerContainer Container { get; private set; }
        protected IInstanceFactoryBuilder InstanceFactoryBuilder { get; private set; }
        protected IInstanceFactory InstanceFactory { get; private set; }
        
        public Type RegisteredType { get; private set; }
        public Type ImplementationType { get; private set; }

        /// <summary>
        /// Check if container registrations have been built.
        /// </summary>
        public bool IsCompiled => InstanceFactory != null;
        public ConstructorInfo Constructor => _constructor.Value;
        public IReadOnlyList<Type> ConstructorArgumentTypes => _constructorArgumentTypes.Value;
        public IReadOnlyList<IRegistration> DependencyRegistrations => _dependencyRegistrations.Value;

        public RegistrationBase(XerContainer container, Type registeredType, Type implementationType)
        {
            Container = container;
            RegisteredType = registeredType;
            ImplementationType = implementationType;

            _constructor = new Lazy<ConstructorInfo>(() => getConstructor(implementationType));

            _constructorArgumentTypes = new Lazy<List<Type>>(() => getTypesOfConstructorArguments(Constructor));

            _dependencyRegistrations = new Lazy<List<IRegistration>>(() => getRegistrationOfDependencies(ConstructorArgumentTypes));

            InstanceFactoryBuilder = CreateInstanceFactoryBuilder(this);
        }

        public virtual object GetInstance()
        {
            var instance = InstanceFactory.CreateInstance();

            if (Container.Options.AllowManualDependencyResolvers)
            {
                IManualDependencyResolver manualResolver = instance as IManualDependencyResolver;
                if (manualResolver != null)
                {
                    manualResolver.ManuallyResolveDependencies(Container);

                    return manualResolver;
                }
            }

            return instance;
        }
        
        protected virtual IInstanceFactoryBuilder CreateInstanceFactoryBuilder(IRegistration registration)
        {
            return new RegistrationInstanceFactoryBuilder(registration);
        }

        /// <summary>
        /// Build instantiator that will produce this registration's instance.
        /// </summary>
        public void Compile()
        {
            if (InstanceFactory == null)
            {
                // Check if we have previously built this factory. If so, use that to save resource.
                InstanceFactory = _instanceFactories[RegisteredType];

                // We have not previously build this factory, so build for the first time and cache.
                if (InstanceFactory == null)
                {
                    // This is a resource intensive operation.
                    InstanceFactory = InstanceFactoryBuilder.BuildInstanceFactory();

                    // Add to cache.
                    _instanceFactories.Add(RegisteredType, InstanceFactory);
                }
            }
        }

        private ConstructorInfo getConstructor(Type implementationType)
        {
            ConstructorInfo constructor = implementationType.GetTypeInfo()
                                                            .DeclaredConstructors
                                                            .FirstOrDefault();
            if (constructor == null)
            {
                throw new ArgumentException("Implementation type has no exposed constructor.", nameof(implementationType));
            }

            return constructor;
        }

        private List<Type> getTypesOfConstructorArguments(ConstructorInfo constructor)
        {
            return constructor.GetParameters()
                              .Select(param => param.ParameterType)
                              .ToList();
        }

        private List<IRegistration> getRegistrationOfDependencies(IEnumerable<Type> dependencyTypes)
        {
            // Get registrations from the container.
            return dependencyTypes.Select(dependencyType =>
            {
                IRegistration registration = Container.GetRegistration(dependencyType);
                if (registration == null)
                {
                    throw new MissingDependencyException(dependencyType, $"{dependencyType.Name} is required by {ImplementationType.Name}, but was not registered in the container.");
                }
                
                return registration;
            })
            .ToList();
        }
    }
}
