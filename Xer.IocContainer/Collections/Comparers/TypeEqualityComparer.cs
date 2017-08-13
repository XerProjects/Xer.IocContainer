using System;
using System.Collections.Generic;
using System.Text;

namespace Xer.IocContainer.Collections
{
    internal class TypeEqualityComparer : EqualityComparerBase<Type>
    {
        public override bool Equals(Type x, Type y)
        {
            return x.Equals(y);
        }
    }
}
