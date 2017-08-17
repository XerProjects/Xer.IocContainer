using System;
using System.Collections.Generic;
using System.Text;
using Xer.IocContainer.Collections;

namespace Xer.IocContainer.LifetimeScopes
{
    public class ControlledLifetimeScope : LifetimeScope
    {
        internal ControlledLifetimeScope(XerContainer container, string scopeName)
            : base(container, scopeName)
        {
        }
    }
}