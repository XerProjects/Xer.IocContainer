using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xer.IocContainer.Attributes;
using Xer.IocContainer.Exceptions;
using Xer.IocContainer.Registrations;
using Xer.IocContainer.Registrations.Dependencies;

namespace Xer.IocContainer.Configuration.PropertySelectors
{
    public abstract class InjectablePropertiesSelector
    {
        public List<PropertyInfo> SelectProperties(TypeInfo implementationTypeInfo)
        {
            return implementationTypeInfo.DeclaredProperties
                    // Only allow properties with public setters.
                    .Where(property => property.SetMethod.IsPublic && IsPropertySupported(property))
                    .ToList();
        }

        protected abstract bool IsPropertySupported(PropertyInfo propertyInfo);
    }
}
