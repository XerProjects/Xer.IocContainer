using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Xer.IocContainer.Configuration.ConstructorSelectors
{
    public abstract class ConstructorSelector
    {
        public abstract ConstructorInfo SelectConstructor(TypeInfo implementationTypeInfo);
    }
}
