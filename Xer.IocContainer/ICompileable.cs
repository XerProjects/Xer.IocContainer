using System;
using System.Collections.Generic;
using System.Text;

namespace Xer.IocContainer
{
    public interface ICompileable
    {
        bool IsCompiled { get; }
        void Compile();
    }
}
