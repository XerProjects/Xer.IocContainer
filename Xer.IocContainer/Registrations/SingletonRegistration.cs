using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xer.IocContainer.Collections;
using Xer.IocContainer.LifetimeScopes;

namespace Xer.IocContainer.Registrations
{
    internal class SingletonRegistration : RegistrationBase
    {
        //private object _instance;

        public override InstanceLifetime InstanceLifetime => InstanceLifetime.Singleton;

        public SingletonRegistration(XerContainer container, Type contractType, Type implementationType)
            : base(container, contractType, implementationType)
        {
        }

        public SingletonRegistration(XerContainer container, Type contractType, object instance)
            : base(container, contractType, instance.GetType())
        {
            // Add to container's singleton scope.
            Container.SingletonScope.AddToScope(this, instance);
        }

        public override object GetInstance()
        {
            return Container.SingletonScope.Resolve(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!IsDisposed)
                {
                    //IDisposable disposableInstance = _instance as IDisposable;
                    //if (disposableInstance != null)
                    //{
                    //    disposableInstance.Dispose();
                        
                    //    _instance = null;
                    //}
                }
            }
        }
    }
}
