using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xer.IocContainer.Collections
{
    internal class EqualityComparerBase<T> : IEqualityComparer<T>
    {
        private class Singleton<T>
        {
            static Singleton()
            {
            }

            public static EqualityComparerBase<T> SingletonInstance = new EqualityComparerBase<T>();
        }

        protected EqualityComparerBase()
        {
        }

        public static EqualityComparerBase<T> Instance
        {
            get { return Singleton<T>.SingletonInstance; }
        }

        public virtual bool Equals(T x, T y)
        {
            return ReferenceEquals(x, y);
        }

        public virtual int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}
