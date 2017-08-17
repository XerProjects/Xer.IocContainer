using System;
using Xer.IocContainer.Attributes;
using Xer.IocContainer.LifetimeScopes;
using Xunit;

namespace Xer.IocContainer.UnitTests
{
    public class ContainerUnitTests
    {
        [Fact]
        public void Resolve()
         {
            var containerBuilder = new XerContainerBuilder(new ContainerOptions());
            containerBuilder.RegisterTransient<ISubTransientService2, SubTestService2>();
            containerBuilder.RegisterTransient<ITransientService1, TestService1>();
            containerBuilder.RegisterTransient<ITransientService2, TestService2>();
            containerBuilder.RegisterSingleton<ISingletonService1, SingletonService1>();
            containerBuilder.RegisterTransient<ITestPropertyService, TestPropertyService>();
            var container = containerBuilder.BuildContainer();

            var singleton1 = container.Resolve<ISingletonService1>();
            var singleton2 = container.Resolve<ISingletonService1>();
            var transient1_1 = container.Resolve<ITransientService1>();
            var transient1_2 = container.Resolve<ITransientService1>();
            //var transient2_1 = container.Resolve<ITransientService2>();
            //var transient2_2 = container.Resolve<ITransientService2>();

            //Assert.Same(singleton1, singleton2);
            //Assert.NotSame(transient1_2, transient1_1);
            //Assert.NotSame(transient2_1, transient2_2);
            //Assert.Same(transient1_1.SingletonService1, transient1_1.SingletonService1);
            //Assert.Same(transient1_1.SingletonService1, transient2_1.SingletonService1);
            //Assert.Same(transient1_1.SingletonService1, transient2_1.SubTransientService1.SingletonService1);
            //Assert.Same(transient2_1.SingletonService1, transient2_1.SubTransientService1.SingletonService1);
            Assert.NotSame(transient1_1, transient1_2);
            Assert.Same(transient1_1.TestPropertyService.TransientService2.SingletonService1, transient1_1.SingletonService1);
        }

        [Fact]
        public void ResolveWithProperty()
        {
            var builder = new XerContainerBuilder(new ContainerOptions());
            builder.RegisterTransient<ISubTransientService2, SubTestService2>();
            builder.RegisterTransient<ITransientService1, TestService1>();
            builder.RegisterTransient<ITransientService2, TestService2>();
            builder.RegisterSingleton<ISingletonService1, SingletonService1>();
            builder.RegisterTransient<ITestPropertyService, TestPropertyService>();

            var container = builder.BuildContainer();
            
            var transient1_1 = container.Resolve<ITransientService1>();
            var transient1_2 = container.Resolve<ITransientService1>();
            
            Assert.NotSame(transient1_1, transient1_2);
        }

        [Fact]
        public void ScopeTest()
        {
            var builder = new XerContainerBuilder(new ContainerOptions());
            builder.RegisterTransient<ISubTransientService2, SubTestService2>();
            builder.RegisterTransient<ITransientService1, TestService1>();
            builder.RegisterTransient<ITransientService2, TestService2>();
            builder.RegisterSingleton<ISingletonService1, SingletonService1>();
            builder.RegisterTransient<ITestPropertyService, TestPropertyService>();
           var container = builder.BuildContainer();

            ITransientService1 ts11;

            using (var scope = container.BeginLifetimeScope())
            {
                ts11 = scope.Resolve<ITransientService1>();
                ITransientService1 ts12 = scope.Resolve<ITransientService1>();
                Assert.Same(ts11, ts12);
            }

            ITransientService1 ts21;

            using (var scope = container.BeginLifetimeScope())
            {
                ts21 = scope.Resolve<ITransientService1>();
                ITransientService1 ts22 = scope.Resolve<ITransientService1>();
                Assert.Same(ts21, ts22);
            }

            Assert.NotSame(ts11, ts21);
        }

        interface ITransientService1
        {
            ISingletonService1 SingletonService1 { get; set; }
            ITransientService2 TransientService2 { get; set; }
            
            ITestPropertyService TestPropertyService { get; set; }
        }

        interface ITransientService2
        {
            ISingletonService1 SingletonService1 { get; set; }
            ISubTransientService2 SubTransientService1 { get; set; }
        }

        interface ISubTransientService2
        {
            ISingletonService1 SingletonService1 { get; set; }
        }

        interface ISingletonService1
        {

        }

        interface ITestPropertyService
        {
            ITransientService2 TransientService2 { get; set; }
        }

        class TestService1 : ITransientService1
        {
            public ISingletonService1 SingletonService1 { get; set; }
            public ITransientService2 TransientService2 { get; set; }
            
            [Inject]
            public ITestPropertyService TestPropertyService { get; set; }

            public TestService1(ISingletonService1 singleton, ITransientService2 service2)
            {
                if (singleton == null)
                    throw new ArgumentNullException(nameof(singleton));

                if(service2 == null)
                    throw new ArgumentNullException(nameof(service2));

                SingletonService1 = singleton;
                TransientService2 = service2;
            }
        }

        class TestService2 : ITransientService2
        {
            public ISingletonService1 SingletonService1 { get; set; }
            public ISubTransientService2 SubTransientService1 { get; set; }

            public TestService2(ISingletonService1 singleton, ISubTransientService2 subService1)
            {
                if (singleton == null)
                    throw new ArgumentNullException(nameof(singleton));

                if (subService1 == null)
                    throw new ArgumentNullException(nameof(subService1));

                SingletonService1 = singleton;
                SubTransientService1 = subService1;
            }
        }

        class SubTestService2 : ISubTransientService2
        {
            public ISingletonService1 SingletonService1 { get; set; }

            public SubTestService2(ISingletonService1 singleton)
            {
                if (singleton == null)
                    throw new ArgumentNullException(nameof(singleton));

                SingletonService1 = singleton;
            }
        }

        class SingletonService1 : ISingletonService1
        {

        }

        class TestPropertyService : ITestPropertyService
        {
            public ITransientService2 TransientService2 { get; set; }

            public TestPropertyService(ITransientService2 service2)
            {
                TransientService2 = service2;
            }
        }
    }
}
