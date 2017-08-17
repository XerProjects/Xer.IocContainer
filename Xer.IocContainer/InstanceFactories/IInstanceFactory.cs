using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xer.IocContainer.LifetimeScopes;

namespace Xer.IocContainer.InstanceFactories
{
    public interface IInstanceFactory
    {
        /// <summary>
        /// Create instance without lifetime scope.
        /// </summary>
        /// <returns>Instance.</returns>
        object CreateInstance();

        /// <summary>
        /// Create instance within the lifetime scope.
        /// </summary>
        /// <param name="lifetimeScope">Lifetime scope.</param>
        /// <returns>Instance.</returns>
        //object CreateInstance(LifetimeScope lifetimeScope);
    }
}
