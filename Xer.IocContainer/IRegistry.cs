using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xer.IocContainer
{
    public interface IRegistry
    {
        void RegisterSingleton<TConcrete>() where TConcrete : class;
        void RegisterSingleton<TContract, TConcrete>() where TConcrete : class, TContract;
        void RegisterSingleton(Type contractType, Type concreteType);
        void RegisterSingleton<TContract>(object instance);
        void RegisterSingleton(object instance);
        void RegisterSingleton(Type instanceType, object instance);

        void RegisterTransient<TConcrete>() where TConcrete : class;
        void RegisterTransient<TContract, TConcrete>() where TConcrete : class, TContract;
        void RegisterTransient(Type contractType, Type concreteType);
    }
}
