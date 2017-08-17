using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xer.IocContainer.LifetimeScopes;
using Xer.IocContainer.Registrations;
using Xer.IocContainer.Registrations.Dependencies;
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

        private static Func<object> createInstanceFactoryForRegistration(IRegistration registration)
        {
            NewExpression newExpression = createNewExpression(registration);

            if (registration.PropertyDependencies.Count > 0)
            {
                MemberInitExpression memberInitExpression = createMemberInitExpression(newExpression, registration);

                // Create lambda that creates an instance with member init.
                return Expression.Lambda<Func<object>>(memberInitExpression,
                                                       registration.RegisteredType.Name,
                                                       Enumerable.Empty<ParameterExpression>())
                                 .Compile();
            }
            else
            {
                // Create lambda that creates an instance.
                return Expression.Lambda<Func<object>>(newExpression,
                                                       registration.RegisteredType.Name,
                                                       Enumerable.Empty<ParameterExpression>())
                                 .Compile();
            }
        }

        private static NewExpression createNewExpression(IRegistration registration)
        {
            NewExpression newExpression;

            if (registration.ConstructorDependencies.Count > 0)
            {
                var callGetInstanceExpressions = 
                    buildCallGetInstanceAnCastResultExpressions(registration);

                // New with parameters.
                newExpression = Expression.New(registration.Constructor, callGetInstanceExpressions);
            }
            else
            {
                // New with no parameters.
                newExpression = Expression.New(registration.Constructor);
            }

            return newExpression;
        }

        private static MemberInitExpression createMemberInitExpression(NewExpression newExpression, IRegistration registration)
        {
            var memberAssignmentExpressions = registration.PropertyDependencies.Select(propertyDependency =>
            {
                Expression getInstanceCallExpression = 
                    buildDependencyGetInstanceExpression(registration, propertyDependency);

                return Expression.Bind(propertyDependency.PropertyInfo.SetMethod, getInstanceCallExpression);
            })
            .ToList();

            return Expression.MemberInit(newExpression, memberAssignmentExpressions);
        }

        private static List<Expression> buildCallGetInstanceAnCastResultExpressions(IRegistration registration)
        {
            return registration.ConstructorDependencies.Select(ctorDependency =>
            {
                return buildDependencyGetInstanceExpression(registration, ctorDependency);
            })
            .ToList();
        }

        private static Expression buildDependencyGetInstanceExpression(IRegistration dependentRegistration, Dependency dependency)
        {
            // Constant.
            // Resulting expression: IRegistration registration = dependency;
            ConstantExpression instanceExpression = Expression.Constant(
                dependency.Registration, 
                RegistrationType);

            // Create expression that call's registration's GetInstance() method.
            // Resulting expression: registration.GetInstance();
            MethodCallExpression callGetInstanceExpression = Expression.Call(instanceExpression, GetInstanceMethod);

            // Cast GetInstance() result to the registration's implementation type.
            // Resulting expression: (TestService)registration.GetInstance();
            return Expression.Convert(callGetInstanceExpression, dependency.Registration.ImplementationType);
        }
    }
}
