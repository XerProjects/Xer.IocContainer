using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Collections.Generic;
using Xer.IocContainer.Exceptions;
using Xer.IocContainer.Registrations;
using Xer.IocContainer.Collections;
using System.Threading.Tasks;
using System.Reflection;

namespace Xer.IocContainer
{
    public class XerContainer : IRegistrar, IResolver, ICompileable
    {
        private readonly RegistrationCollection _registrations;

        /// <summary>
        /// Options.
        /// </summary>
        public ContainerOptions Options { get; private set; }

        /// <summary>
        /// Check to see if container is already compiled.
        /// </summary>
        public bool IsCompiled { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public XerContainer()
        {
            _registrations = new RegistrationCollection();

            Options = new ContainerOptions();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">Container options.</param>
        public XerContainer(ContainerOptions options)
        {
            _registrations = new RegistrationCollection();

            Options = options;
        }

        public static XerContainer Create(Action<IRegistrar> configure, ContainerOptions options)
        {
            XerContainer container = new XerContainer(options);

            // Configure.
            configure.Invoke(container);

            // Compile.
            container.Compile();

            return container;
        }

        #region Resolve

        /// <summary>
        /// Get an instance of a registered type which matched the specified type.
        /// </summary>
        /// <typeparam name="TContract">Type to resolve.</typeparam>
        /// <returns>Instance of the specified type. This will return null if type is not registered.</returns>
        public TContract Resolve<TContract>()
        {
            Type type = typeof(TContract);

            return (TContract)Resolve(type);
        }

        /// <summary>
        /// Get an instance of a registered type which matched the specified type.
        /// </summary>
        /// <param name="contractType">Type to resolve.</param>
        /// <returns>Instance of the specified type. This will return null if type is not registered.</returns>
        public object Resolve(Type contractType)
        {
            IRegistration registration = _registrations[contractType];

            if (registration == null)
            {
                return null;
            }

            return registration.GetInstance();
        }

        #endregion Resolve

        #region Register Singleton

        /// <summary>
        /// Register object of given type as singleton.
        /// </summary>
        /// <typeparam name="TConcrete">Type which will be used in resolving an instance of this object.</typeparam>
        public void RegisterSingleton<TConcrete>() where TConcrete : class
        {
            Type concreteType = typeof(TConcrete);

            RegisterSingleton(concreteType, concreteType);
        }

        /// <summary>
        /// Register object of given types as singleton.
        /// </summary>
        /// <typeparam name="TContract">Interface which will be used in resolving an instance of this object.</typeparam>
        /// <typeparam name="TConcrete">Type which implements TContract.</typeparam>
        public void RegisterSingleton<TContract, TConcrete>() where TConcrete : class, TContract
        {
            RegisterSingleton(typeof(TContract), typeof(TConcrete));
        }
        /// <summary>
        /// Register object of given types as singleton.
        /// </summary>
        /// <param name="contractType">Type which will be used in resolving an instance of this object.</param>
        /// <param name="concreteType">Type which implements TContract.</param>
        public void RegisterSingleton(Type contractType, Type concreteType)
        {
            if (!isTypeAssignableFrom(concreteType, contractType))
            {
                throw new ArgumentException($"{concreteType.Name} does not implement the {contractType.Name} interface.", nameof(concreteType));
            }

            addRegistration(new SingletonRegistration(this, contractType, concreteType));
        }

        /// <summary>
        /// Register given instance as singleton.
        /// </summary>
        /// <typeparam name="IContract">Interface which will be used in resolving the instance.</typeparam>
        /// <param name="instance">Instance of the given TContract.</param>
        public void RegisterSingleton<IContract>(object instance)
        {
            RegisterSingleton(typeof(IContract), instance);
        }

        /// <summary>
        /// Register given instance as singleton. An instance of this object can be resolved using its type.
        /// </summary>
        /// <param name="instance">Instance to register.</param>
        public void RegisterSingleton(object instance)
        {
            RegisterSingleton(instance.GetType(), instance);
        }

        /// <summary>
        /// Register given instance as singleton. An instance of this object can be resolved using the given contract type.
        /// </summary>
        /// <param name="contractType">Type which will be used in resolving the instance.</param>
        /// <param name="instance">Instance to register.</param>
        public void RegisterSingleton(Type contractType, object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (!isTypeAssignableFrom(instance.GetType(), contractType))
            {
                throw new ArgumentException($"{instance.GetType().Name} instance does not implement the {contractType.Name} interface.", nameof(instance));
            }

            addRegistration(new SingletonRegistration(this, contractType, instance));
        }

        #endregion Register Singleton

        #region Register Transient

        /// <summary>
        /// Register object of given type as transient.
        /// </summary>
        /// <typeparam name="TConcrete">Type which will be used in resolving an instance.</typeparam>
        public void RegisterTransient<TConcrete>() where TConcrete : class
        {
            Type concreteType = typeof(TConcrete);

            RegisterTransient(concreteType, concreteType);
        }

        /// <summary>
        /// Register object of givens type as transient.
        /// </summary>
        /// <typeparam name="TContract">Interface which will be used in resolving an instance.</typeparam>
        /// <typeparam name="TConcrete">Type which implements TContract.</typeparam>
        public void RegisterTransient<TContract, TConcrete>() where TConcrete : class, TContract
        {
            RegisterTransient(typeof(TContract), typeof(TConcrete));
        }

        /// <summary>
        /// Register object of givens type as transient.
        /// </summary>
        /// <param name="contractType">Type which will be used in resolving an instance.</param>
        /// <param name="concreteType">Type which implements TContract.</param>
        public void RegisterTransient(Type contractType, Type concreteType)
        {
            if (!isTypeAssignableFrom(concreteType, contractType))
            {
                throw new ArgumentException($"{concreteType.Name} does not implement the {contractType.Name} interface.", nameof(concreteType));
            }

            addRegistration(new TransientRegistration(this, contractType, concreteType));
        }

        #endregion Register Transient

        #region Compile

        /// <summary>
        /// It is important that be we resolve anything, 
        /// we need to compile the container to build the internal instance factories,
        /// which are responsible in generating instances.
        /// </summary>
        public void Compile()
        {
            foreach (IRegistration registration in _registrations)
            {
                if (!registration.IsCompiled)
                {
                    registration.Compile();
                }
            }

            //Parallel.ForEach(_registrations,
            //                 new ParallelOptions() { MaxDegreeOfParallelism = 50 },
            //                 reg =>
            //                 {
            //                     if (!reg.IsCompiled)
            //                     {
            //                         reg.Compile();
            //                     }
            //                 });

            // Set to compiled.
            IsCompiled = true;
        }

        #endregion Compile

        #region Internal Methods

        /// <summary>
        /// Get internal registration.
        /// </summary>
        /// <param name="contractType">Registered type of the registration.</param>
        /// <returns>Instance of registration.</returns>
        internal IRegistration GetRegistration(Type contractType)
        {
            return _registrations[contractType];
        }

        /// <summary>
        /// Get internal registration.
        /// </summary>
        /// <typeparam name="TRegistered">Registered type of the registration.</typeparam>
        /// <returns>Instance of registration.</returns>
        internal IRegistration GetRegistration<TRegistered>()
        {
            return _registrations[typeof(TRegistered)];
        }

        #endregion Internal Methods

        #region Functions

        /// <summary>
        /// Add registration to cache. 
        /// </summary>
        /// <param name="registration">Registration to add.</param>
        private void addRegistration(IRegistration registration)
        {
            if (typeIsRegistered(registration.RegisteredType))
            {
                if (Options.ThrowExceptionOnDuplicateRegistration)
                {
                    Type type = registration.RegisteredType;

                    throw new DuplicateRegistrationException(type, $"{type.Name} is already registered.");
                }
            }

            _registrations.Add(registration.RegisteredType, registration);
        }

        /// <summary>
        /// Validate if type is not previously registered to the container.
        /// </summary>
        /// <param name="contractType">Type to register.</param>
        /// <returns>True, if type is already registered. Otherwise, false.</returns>
        private bool typeIsRegistered(Type contractType)
        {
            return _registrations.Contains(contractType);
        }

        /// <summary>
        /// Check if a type is assignable to another type.
        /// </summary>
        /// <param name="to">Type to assign into another type.</param>
        /// <param name="from">Type be assigned into.</param>
        /// <returns>True, if "to" can be assigned into "from". Otherwise, false.</returns>
        private bool isTypeAssignableFrom(Type to, Type from)
        {
            return from != to && !to.GetTypeInfo().IsAssignableFrom(from.GetTypeInfo());
        }

        #endregion Functions
    }
}
