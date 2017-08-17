using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xer.IocContainer.Attributes;
using Xer.IocContainer.Registrations;

namespace Xer.IocContainer.Configuration.PropertySelectors
{
    public class PropertiesMarkedWithInjectAttributeSelector : InjectablePropertiesSelector
    {
        protected override bool IsPropertySupported(PropertyInfo propertyInfo)
        {
            return propertyInfo.CustomAttributes.Any(attr => attr.AttributeType == typeof(InjectAttribute));
        }
    }
}
