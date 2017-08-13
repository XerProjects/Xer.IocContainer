using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xer.IocContainer.InstanceFactories;

namespace Xer.IocContainer.Registrations
{
    internal interface IRegistration : ICompileable
    {
        /// <summary>
        /// Constructor of this registered object.
        /// </summary>
        ConstructorInfo Constructor { get; }

        /// <summary>
        /// Registered type of this registered object.
        /// </summary>
        Type RegisteredType { get; }

        /// <summary>
        /// Implementation type of this registered object.
        /// </summary>
        Type ImplementationType { get; }

        /// <summary>
        /// Constructor dependencies of this registered object.
        /// </summary>
        IReadOnlyList<Type> ConstructorArgumentTypes { get; }

        /// <summary>
        /// Registrations of this object's dependencies.
        /// </summary>
        IReadOnlyList<IRegistration> DependencyRegistrations { get; }

        /// <summary>
        /// Registration type.
        /// </summary>
        RegistrationType RegistrationType { get; }

        /// <summary>
        /// Get an instance of this registered object.
        /// </summary>
        /// <param name="resolver">Object resolver that is used to resolve dependencies.</param>
        /// <returns>Instance of this registered object.</returns>
        object GetInstance();
    }
}
