using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xer.IocContainer.Attributes;

namespace Xer.IocContainer.Configuration.ConstructorSelectors
{
    public class FirstConstructorMarkedWithInjectSelector : ConstructorSelector
    {
        public override ConstructorInfo SelectConstructor(TypeInfo implementationTypeInfo)
        {
            return implementationTypeInfo.DeclaredConstructors.FirstOrDefault(ctor => ctor.GetCustomAttributes<InjectAttribute>().Any());
        }
    }
}
