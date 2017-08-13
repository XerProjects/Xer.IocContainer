using System;
using System.Collections.Generic;
using System.Text;

namespace Xer.IocContainer.Exceptions
{
    public class MissingDependencyException : Exception
    {
        public Type MissingDependency { get; private set; }

        public MissingDependencyException(Type missingDependency, string message) 
            : this(missingDependency, message, null)
        {
        }

        public MissingDependencyException(Type missingDependency, string message, Exception innerException)
            : base(message, innerException)
        {
            MissingDependency = missingDependency;
        }
    }
}
