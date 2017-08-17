using System;
using System.Collections.Generic;
using System.Text;
using Xer.IocContainer.Collections;
using Xer.IocContainer.Registrations;

namespace Xer.IocContainer.LifetimeScopes
{
    public abstract class LifetimeScope : IDisposable
    {
        private readonly XerContainer _container;
        private readonly InstanceCollection<Type> _scopeInstances = new InstanceCollection<Type>();

        public string ScopeName { get; private set; }
        public bool IsDisposed { get; private set; }

        internal LifetimeScope(XerContainer container, string scopeName)
        {
            _container = container;
            ScopeName = scopeName;
        }

        internal object CreateScopedInstance(IRegistration registration)
        {
            object instance = registration.InstanceFactory.CreateInstance();

            if (instance != null)
            {
                AddToScope(registration, instance);
            }

            return instance;
        }

        internal object Resolve(IRegistration registration)
        {
            object instance = Resolve(registration.RegisteredType);

            if (instance == null)
            {
                instance = CreateScopedInstance(registration);
            }
            
            return instance;
        }

        internal void AddToScope(IRegistration registration, object instance)
        {
            if (!_scopeInstances.Contains(registration.RegisteredType))
            {
                _scopeInstances.Add(registration.RegisteredType, instance);
            }
        }

        internal bool RemoveFromScope(IRegistration registration)
        {
            return _scopeInstances.Remove(registration.RegisteredType);
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public virtual object Resolve(Type contractType)
        {
            object instance;

            if (!_scopeInstances.TryGetValue(contractType, out instance))
            {
                IRegistration registration = _container.GetRegistration(contractType);

                instance = CreateScopedInstance(registration);
            }

            return instance;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO: Research.
                // Dispose disposable instances in this scope.
                foreach (object instance in _scopeInstances)
                {
                    IDisposable disposableInstance = instance as IDisposable;
                    if (disposableInstance != null)
                    {
                        disposableInstance.Dispose();
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
