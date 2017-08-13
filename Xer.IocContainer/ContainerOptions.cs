﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xer.IocContainer
{
    public class ContainerOptions
    {
        public bool ThrowExceptionOnDuplicateRegistration { get; set; } = false;
        public bool AllowManualDependencyResolvers { get; set; } = true;
    }
}
