using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xer.IocContainer.LifetimeScopes;

namespace Xer.IocContainer
{
    public interface IResolver
    {
        TContract Resolve<TContract>();
        object Resolve(Type contractType);
    }
}
