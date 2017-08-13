﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xer.IocContainer.InstanceFactories.Builders
{
    internal interface IInstanceFactoryBuilder
    {
        IInstanceFactory BuildInstanceFactory();
    }
}
