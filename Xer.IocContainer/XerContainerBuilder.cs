using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xer.IocContainer
{
    public class XerContainerBuilder : IRegistry
    {
        private readonly XerContainer _container;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">Container options.</param>
        public XerContainerBuilder(ContainerOptions options)
        {
            _container = new XerContainer(options);
        }

        public void RegisterSingleton<TConcrete>() where TConcrete : class
        {
            _container.RegisterSingleton<TConcrete>();
        }

        public void RegisterSingleton<TContract, TConcrete>() where TConcrete : class, TContract
        {
            _container.RegisterSingleton<TContract, TConcrete>();
        }

        public void RegisterSingleton(Type contractType, Type concreteType)
        {
            _container.RegisterSingleton(contractType, concreteType);
        }

        public void RegisterSingleton<TContract>(object instance)
        {
            _container.RegisterSingleton(typeof(TContract), instance);
        }

        public void RegisterSingleton(object instance)
        {
            _container.RegisterSingleton(instance);
        }

        public void RegisterSingleton(Type instanceType, object instance)
        {
            _container.RegisterSingleton(instanceType, instance);
        }

        public void RegisterTransient<TConcrete>() where TConcrete : class
        {
            _container.RegisterTransient<TConcrete>();
        }

        public void RegisterTransient<TContract, TConcrete>() where TConcrete : class, TContract
        {
            _container.RegisterTransient<TContract, TConcrete>();
        }

        public void RegisterTransient(Type contractType, Type concreteType)
        {
            _container.RegisterTransient(contractType, concreteType);
        }

        /// <summary>
        /// Create the container.
        /// </summary>
        /// <returns>Instance of the container.</returns>
        public XerContainer BuildContainer()
        {
            // Compile.
            _container.Compile();

            return _container;
        }
    }
}
