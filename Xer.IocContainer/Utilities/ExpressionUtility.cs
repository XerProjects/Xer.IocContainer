using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xer.IocContainer.Utilities
{
    internal static class ExpressionUtility
    {
        internal static MethodCallExpression GetMethodCallExpression<T>(Expression<Func<T>> exp)
        {
            return exp.Body as MethodCallExpression;
        }

        internal static MethodInfo GetMethodInfo<T>(Expression<Action<T>> exp)
        {
            var member = exp.Body as MethodCallExpression;
            if (member != null)
            {
                return member.Method;
            }

            return null;
        }
    }
}
