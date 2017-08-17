//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Text;
//using Xer.IocContainer.LifetimeScopes;
//using Xer.IocContainer.Registrations;
//using Xer.IocContainer.Registrations.Dependencies;
//using Xer.IocContainer.Utilities;

//namespace Xer.IocContainer.InstanceFactories
//{
//    public class ScopedRegistrationInstanceFactory : IInstanceFactory
//    {
//        private readonly IRegistration _registration;
//        private readonly Func<LifetimeScope, object> _instanceFactory;

//        // Cache.
//        private static readonly MethodInfo GetInstanceMethod = ExpressionUtility.GetMethodInfo<IRegistration>((r) => r.GetInstance());
//        private static readonly Type IRegistrationType = typeof(IRegistration);
//        private static readonly ParameterExpression LifetimeScopeParameterExpression = Expression.Parameter(typeof(LifetimeScope), "scope");

//        public ScopedRegistrationInstanceFactory(IRegistration registration)
//        {
//            _registration = registration;
//            _instanceFactory = createInstanceFactoryForRegistration(registration);
//        }

//        public object CreateInstance()
//        {
//            return _instanceFactory.Invoke(new NoLifetimeScope(_registration.Container, string.Empty));
//        }

//        public object CreateInstance(LifetimeScope scope)
//        {
//            return _instanceFactory.Invoke(scope);
//        }

//        private Func<LifetimeScope, object> createInstanceFactoryForRegistration(IRegistration registration)
//        {
//            BlockExpression blockExpression;
//            NewExpression newExpression = createNewExpression(registration);

//            if (registration.PropertyDependencies.Count > 0)
//            {
//                MemberInitExpression memberInitExpression = createMemberInitExpression(newExpression, registration);

//                // (lifetimeScope) => (TestService)registration.GetInstance(lifetimeScope);
//                blockExpression = Expression.Block(
//                    new[] { LifetimeScopeParameterExpression },
//                    memberInitExpression);
//            }
//            else
//            {
//                blockExpression = Expression.Block(
//                    new[] { LifetimeScopeParameterExpression },
//                    newExpression);
//            }

//            // Create lambda that creates an instance with member init.
//            return Expression.Lambda<Func<LifetimeScope, object>>(blockExpression,
//                                                                  registration.RegisteredType.Name,
//                                                                  new[] { LifetimeScopeParameterExpression })
//                             .Compile();
//        }

//        private static NewExpression createNewExpression(IRegistration registration)
//        {
//            NewExpression newExpression;

//            if (registration.ConstructorDependencies.Count > 0)
//            {
//                List<UnaryExpression> getInstanceCallExpressions = buildGetInstanceWithCastExpressions(registration);

//                // New with parameters.
//                newExpression = Expression.New(registration.Constructor, getInstanceCallExpressions);
//            }
//            else
//            {
//                // New with no parameters.
//                newExpression = Expression.New(registration.Constructor);
//            }

//            return newExpression;
//        }

//        private MemberInitExpression createMemberInitExpression(NewExpression newExpression, IRegistration registration)
//        {
//            List<MemberAssignment> memberAssignmentExpressions = registration.PropertyDependencies
//            .Select(propertyDependency =>
//            {
//                Expression getInstanceCallExpression = buildGetInstanceAndCastExpression(registration, propertyDependency);

//                return Expression.Bind(propertyDependency.PropertyInfo.SetMethod, getInstanceCallExpression);
//            })
//            .ToList();

//            return Expression.MemberInit(newExpression, memberAssignmentExpressions);
//        }

//        private static List<UnaryExpression> buildGetInstanceWithCastExpressions(IRegistration registration)
//        {
//            return registration.ConstructorDependencies.Select(ctorDependency =>
//            {
//                return buildGetInstanceAndCastExpression(registration, ctorDependency);
//            })
//            .ToList();
//        }

//        private static UnaryExpression buildGetInstanceAndCastExpression(IRegistration parent, Dependency dependency)
//        {
//            return Expression.Convert(
//                        Expression.Call(
//                            Expression.Constant(new ScopedRegistration(dependency.Registration, parent.Container.LifetimeScopeManager.GetOrCreateScope(parent.RegisteredType.Name)), IRegistrationType),
//                            GetInstanceMethod,
//                            LifetimeScopeParameterExpression
//                        ),
//                    dependency.Registration.ImplementationType);

//            //// Constant.
//            //// Resulting expression: IRegistration registration = dependency;
//            //ConstantExpression instanceExpression = Expression.Constant(registration, RegistrationType);

//            //// Create expression that call's registration's GetInstance() method.
//            //// Resulting expression: registration.GetInstance();
//            //MethodCallExpression callGetInstanceExpression = Expression.Call(instanceExpression, GetInstanceMethod);

//            //// Cast GetInstance() result to the registration's implementation type.
//            //// Resulting expression: (TestService)registration.GetInstance();
//            //return Expression.Convert(callGetInstanceExpression, registration.ImplementationType);
//        }
//    }
//}
