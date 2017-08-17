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
using Xer.IocContainer.Registrations.Dependencies;
using Xer.IocContainer.Attributes;
using Xer.IocContainer.LifetimeScopes;

namespace Xer.IocContainer.Registrations
{
    internal abstract class RegistrationBase : IRegistration, IDisposable
    {
        /// <summary>
        /// Cache compiled instance factory so that we can skip building the factory over and over.
        /// </summary>
        private static readonly InstanceFactoryCollection _instanceFactories = new InstanceFactoryCollection();

        private readonly Lazy<ConstructorInfo> _constructor;
        private readonly Lazy<List<PropertyInfo>> _injectableProperties;
        private readonly Lazy<List<ConstructorDependency>> _constructorDependencies;
        private readonly Lazy<List<PropertyDependency>> _propertyDependencies;
        
        /// <summary>
        /// Registration's lifetime.
        /// </summary>
        public abstract InstanceLifetime InstanceLifetime { get; }

        /// <summary>
        /// Flag that indicates if this object has been disposed.
        /// </summary>
        protected virtual bool IsDisposed { get; private set; }

        /// <summary>
        /// Check if container registrations have been built.
        /// </summary>
        public bool IsCompiled => InstanceFactory != null;

        /// <summary>
        /// Instance factory builder.
        /// </summary>
        protected IInstanceFactoryBuilder InstanceFactoryBuilder { get; private set; }

        /// <summary>
        /// Creates an instance of this registration.
        /// </summary>
        public IInstanceFactory InstanceFactory { get; private set; }
        
        /// <summary>
        /// Type in which an instance of this registration will be resolved.
        /// </summary>
        public Type RegisteredType { get; private set; }

        /// <summary>
        /// Contract's Type Info.
        /// </summary>
        public TypeInfo RegisteredTypeInfo { get; private set; }

        /// <summary>
        /// Concrete type which is used to an create instance.
        /// </summary>
        public Type ImplementationType { get; private set; }

        /// <summary>
        /// Implementation's Type Info.
        /// </summary>
        public TypeInfo ImplementationTypeInfo { get; private set; }

        /// <summary>
        /// IoC Container.
        /// </summary>
        public XerContainer Container { get; private set; }

        /// <summary>
        /// Selected constructor of this registered object.
        /// </summary>
        public ConstructorInfo Constructor => _constructor.Value;

        /// <summary>
        /// Properties of this registered object that are selected for injection.
        /// </summary>
        public IReadOnlyList<PropertyInfo> InjectableProperties => _injectableProperties.Value;

        /// <summary>
        /// Constructor dependencies.
        /// </summary>
        public IReadOnlyList<ConstructorDependency> ConstructorDependencies => _constructorDependencies.Value;

        /// <summary>
        /// Property dependencies.
        /// </summary>
        public IReadOnlyList<PropertyDependency> PropertyDependencies => _propertyDependencies.Value;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="container">Container.</param>
        /// <param name="registeredType">Registered type.</param>
        /// <param name="implementationType">Implementation type.</param>
        public RegistrationBase(XerContainer container, Type registeredType, Type implementationType)
        {
            Container = container;
            RegisteredType = registeredType;
            RegisteredTypeInfo = registeredType.GetTypeInfo();
            ImplementationType = implementationType;
            ImplementationTypeInfo = implementationType.GetTypeInfo();

            _constructor = LazyLoad(() => 
                Container.Options.ConstructorSelector.SelectConstructor(ImplementationTypeInfo));

            _injectableProperties = LazyLoad(() => 
                Container.Options.InjectablePropertiesSelector.SelectProperties(ImplementationTypeInfo));

            _constructorDependencies = LazyLoad(() => 
                GetConstructorDependencies(Constructor));

            _propertyDependencies = LazyLoad(() => 
                GetPropertyDependencies(InjectableProperties));

            // Create builder.
            InstanceFactoryBuilder = CreateInstanceFactoryBuilder(this);
        }

        /// <summary>
        /// Get an instance of this registration's type.
        /// </summary>
        /// <returns>Instance of this registration's type.</returns>
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

        /// <summary>
        /// Create instance creator builder.
        /// </summary>
        /// <param name="registration">Registration to create instance from.</param>
        /// <returns>Instance creator builder.</returns>
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
                IInstanceFactory factory;

                // Check if we have previously built this factory. If so, reuse that to save resource.
                if (_instanceFactories.TryGetValue(RegisteredType, out factory))
                {
                    InstanceFactory = factory;
                }
                else
                {
                    // We have not previously build this factory, so build for the first time and cache.
                    // This is a resource intensive operation.
                    InstanceFactory = InstanceFactoryBuilder.BuildInstanceFactory();

                    // Add to cache.
                    _instanceFactories.Add(RegisteredType, InstanceFactory);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!IsDisposed)
            {
                // TODO: Dispose.

                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private List<ConstructorDependency> GetConstructorDependencies(ConstructorInfo constructor)
        {
            return constructor.GetParameters().Select(param =>
            {
                IRegistration ctorDependencyRegistration = Container.GetRegistration(param.ParameterType);
                if (ctorDependencyRegistration == null)
                {
                    throw new MissingDependencyException(param.ParameterType,
                        $"{param.ParameterType.Name} is required by {ImplementationType.Name}, but was not registered in the container.");
                }

                return new ConstructorDependency(Constructor, ctorDependencyRegistration);
            })
            .ToList();
        }

        private List<PropertyDependency> GetPropertyDependencies(IEnumerable<PropertyInfo> injectableProperties)
        {
            return injectableProperties.Select(prop =>
            {
                IRegistration propDependencyRegistration = Container.GetRegistration(prop.PropertyType);
                if (propDependencyRegistration == null)
                {
                    throw new MissingDependencyException(prop.PropertyType,
                        $"{prop.PropertyType.Name} is required by {ImplementationType.Name}, but was not registered in the container.");
                }

                return new PropertyDependency(prop, propDependencyRegistration);
            })
            .ToList();
        }

        private Lazy<T> LazyLoad<T>(Func<T> factory)
        {
            return new Lazy<T>(factory);
        }
    }
}
