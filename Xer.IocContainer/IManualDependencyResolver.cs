using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xer.IocContainer
{
    /// <summary>
    /// Allow implementor of this interface to manually resolve its internal dependencies.
    /// </summary>
    public interface IManualDependencyResolver
    {
        /// <summary>
        /// Allow this object to manually resolve its internal dependencies.
        /// This will only work if object was also resolved through the IOC container.
        /// </summary>
        /// <param name="resolver">Resolver where objects can resolve their internal dependencies.</param>
        void ManuallyResolveDependencies(IResolver resolver);
    }
}
