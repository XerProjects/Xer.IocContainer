using System;
using System.Collections.Generic;
using System.Text;

namespace Xer.IocContainer.Registrations
{
    public enum InstanceLifetime
    {
        Singleton,
        Transient,
        Scoped
    }
}
