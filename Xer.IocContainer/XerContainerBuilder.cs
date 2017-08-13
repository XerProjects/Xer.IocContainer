using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xer.IocContainer
{
    public class XerContainerBuilder
    {
        private List<Action<XerContainer>> _registrationBuilders;
        private readonly XerContainer _container;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">Container options.</param>
        public XerContainerBuilder(ContainerOptions options)
        {
            _registrationBuilders = new List<Action<XerContainer>>();
            _container = new XerContainer(options);
        }

        /// <summary>
        /// Register types to the registration manager.
        /// </summary>
        /// <param name="registrationBuilder">Action to register types to the registration manager.</param>
        /// <returns>Same instance of this container builder.</returns>
        public XerContainerBuilder Configure(Action<IRegistrar> registrationBuilder)
        {
            _registrationBuilders.Add(registrationBuilder);

            return this;
        }

        //public T Configure<T>(Func<IRegistrationManager, T> registrationBuilder)
        //{
        //    return registrationBuilder.Invoke(_container);
        //}

        /// <summary>
        /// Create and compile the container.
        /// </summary>
        /// <returns>Instance of the compiled container.</returns>
        public XerContainer BuildContainer()
        {
            _registrationBuilders.ForEach(cm => cm.Invoke(_container));

            // Compile.
            _container.Compile();

            return _container;
        }
    }
}
