using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xer.IocContainer.Collections;

namespace Xer.IocContainer.Registrations
{
    internal class SingletonRegistration : RegistrationBase
    {
        private static readonly SingletonInstanceCollection _singletonInstances = new SingletonInstanceCollection();

        private object _instance;

        public override RegistrationType RegistrationType => RegistrationType.Singleton;

        public SingletonRegistration(XerContainer container, Type contractType, Type implementationType)
            : base(container, contractType, implementationType)
        {
        }

        public SingletonRegistration(XerContainer container, Type contractType, object instance)
            : base(container, contractType, instance.GetType())
        {
            addInstanceToCache(contractType, instance);
        }

        public override object GetInstance()
        {
            // object instance = _singletonInstances[RegisteredType];
            // Check if already instantiated.
            if (_instance == null)
            {
                // Instantiate.
                _instance = base.GetInstance();

                addInstanceToCache(RegisteredType, _instance);
            }

            return _instance;
        }

        private static void addInstanceToCache(Type registeredType, object instance)
        {
            if (!_singletonInstances.Contains(registeredType))
            {
                // Cache.
                _singletonInstances.Add(registeredType, instance);
            }
        }
    }
}
