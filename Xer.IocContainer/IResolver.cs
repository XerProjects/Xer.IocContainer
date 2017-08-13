using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xer.IocContainer
{
    public interface IResolver
    {
        TObject Resolve<TObject>();
        object Resolve(Type registeredObjectType);
    }
}
