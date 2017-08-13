using System;
using System.Collections.Generic;
using System.Text;

namespace Xer.IocContainer
{
    internal interface ICompileable
    {
        bool IsCompiled { get; }
        void Compile();
    }
}
