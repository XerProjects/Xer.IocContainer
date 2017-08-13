using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xer.IocContainer.Exceptions
{
    public class DuplicateRegistrationException : Exception
    {
        public Type TypeBeingRegistered { get; private set; }
        
        public DuplicateRegistrationException(Type typeBeingRegistered, string message) 
            : base(message)
        {
            TypeBeingRegistered = typeBeingRegistered;
        }

        public DuplicateRegistrationException(Type typeBeingRegistered, string message, Exception innerException) 
            : base(message, innerException)
        {
            TypeBeingRegistered = typeBeingRegistered;
        }
    }
}
