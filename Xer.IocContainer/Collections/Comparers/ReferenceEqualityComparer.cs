using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xer.IocContainer.Collections
{
    internal class ReferenceEqualityComparer<T> : IEqualityComparer<T>
    {
        private class Singleton<TSingleton>
        {
            static Singleton()
            {
            }

            public readonly static ReferenceEqualityComparer<TSingleton> SingletonInstance = new ReferenceEqualityComparer<TSingleton>();
        }

        protected ReferenceEqualityComparer()
        {
        }

        public static ReferenceEqualityComparer<T> Instance
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
