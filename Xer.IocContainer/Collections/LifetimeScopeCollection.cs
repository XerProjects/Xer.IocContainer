using System;
using System.Collections.Generic;
using System.Text;
using Xer.IocContainer.LifetimeScopes;

namespace Xer.IocContainer.Collections
{
    internal class LifetimeScopeCollection : KeyValueCollectionBase<string, LifetimeScope>
    {
        protected override IEqualityComparer<string> KeyEqualityComparer => StringComparer.Ordinal;
    }
}
