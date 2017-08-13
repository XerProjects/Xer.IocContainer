using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xer.IocContainer.Registrations;
using Xer.IocContainer.Utilities;

namespace Xer.IocContainer.InstanceFactories
{
    internal class RegistrationInstanceFactory : IInstanceFactory
    {
        private readonly IRegistration _registration;
        private readonly Func<object> _instanceFactory;

        // Cache.
        private static readonly MethodInfo GetInstanceMethod = ExpressionUtility.GetMethodInfo<IRegistration>((r) => r.GetInstance());
        private static readonly Type RegistrationType = typeof(IRegistration);

        public RegistrationInstanceFactory(IRegistration registration)
        {
            _registration = registration;
            _instanceFactory = createInstanceFactoryForRegistration(registration);
        }

        public object CreateInstance()
        {
            return _instanceFactory.Invoke();
        }

        private Func<object> createInstanceFactoryForRegistration(IRegistration registration)
        {
            NewExpression newExpression;

            if (registration.DependencyRegistrations.Count > 0)
            {
                IEnumerable<UnaryExpression> getInstanceCallExpressions = buildGetInstanceAndCastExpressions(registration);

                // New with parameters.
                newExpression = Expression.New(registration.Constructor, getInstanceCallExpressions);
            }
            else
            {
                // New with no parameters.
                newExpression = Expression.New(registration.Constructor);
            }

            // Create lambda that creates an instance.
            return Expression.Lambda<Func<object>>(newExpression, 
                                                   registration.RegisteredType.Name, 
                                                   Enumerable.Empty<ParameterExpression>())
                             .Compile();
        }

        private static List<UnaryExpression> buildGetInstanceAndCastExpressions(IRegistration registration)
        {
            return registration.DependencyRegistrations.Select(dependency =>
            {
                // Constant.
                // Resulting expression: IRegistration registration = dependency;
                ConstantExpression instanceExpression = Expression.Constant(dependency, RegistrationType);

                // Create expression that call's registration's GetInstance() method.
                // Resulting expression: registration.GetInstance();
                MethodCallExpression callGetInstanceExpression = Expression.Call(instanceExpression, GetInstanceMethod);

                // Cast GetInstance() result to the registration's implementation type.
                // Resulting expression: (TestService)registration.GetInstance();
                return Expression.Convert(callGetInstanceExpression, dependency.ImplementationType);
            })
            .ToList();
        }
    }
}
