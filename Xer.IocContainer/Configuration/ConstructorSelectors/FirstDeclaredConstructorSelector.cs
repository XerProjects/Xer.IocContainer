using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Xer.IocContainer.Configuration.ConstructorSelectors
{
    public class FirstDeclaredConstructorSelector : ConstructorSelector
    {
        public override ConstructorInfo SelectConstructor(TypeInfo implementationTypeInfo)
        {
            return implementationTypeInfo.DeclaredConstructors.FirstOrDefault();
        }
    }
}
