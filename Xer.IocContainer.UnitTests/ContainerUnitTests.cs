using System;
using Xunit;

namespace Xer.IocContainer.UnitTests
{
    public class ContainerUnitTests
    {
        [Fact]
        public void Resolve()
        {
            var container = new XerContainerBuilder(new ContainerOptions())
            .Configure(r =>
            {
                r.RegisterTransient<ISubTransientService1, SubTestService1>();
                r.RegisterTransient<ITransientService1, TestService1>();
                r.RegisterTransient<ITransientService2, TestService2>();
                r.RegisterSingleton<ISingletonService1, SingletonService1>();
            })
            .BuildContainer();

            var singleton1 = container.Resolve<ISingletonService1>();
            var singleton2 = container.Resolve<ISingletonService1>();
            var transient1_1 = container.Resolve<ITransientService1>();
            var transient1_2 = container.Resolve<ITransientService1>();
            var transient2_1 = container.Resolve<ITransientService2>();
            var transient2_2 = container.Resolve<ITransientService2>();

            Assert.Same(singleton1, singleton2);
            Assert.NotSame(transient1_1, transient2_1);
            Assert.NotSame(transient2_1, transient2_2);
        }

        interface ITransientService1
        {

        }

        interface ITransientService2
        {

        }

        interface ISubTransientService1
        {

        }

        interface ISingletonService1
        {

        }

        class TestService1 : ITransientService1
        {
            public TestService1(ISingletonService1 singleton, ITransientService2 service2)
            {
                if (singleton == null)
                    throw new ArgumentNullException(nameof(singleton));

                if(service2 == null)
                    throw new ArgumentNullException(nameof(service2));
            }
        }

        class TestService2 : ITransientService2
        {
            public TestService2(ISingletonService1 singleton, ISubTransientService1 subService1)
            {
                if (singleton == null)
                    throw new ArgumentNullException(nameof(singleton));

                if (subService1 == null)
                    throw new ArgumentNullException(nameof(subService1));
            }
        }

        class SubTestService1 : ISubTransientService1
        {
            public SubTestService1(ISingletonService1 singleton)
            {
                if (singleton == null)
                    throw new ArgumentNullException(nameof(singleton));
            }
        }

        class SingletonService1 : ISingletonService1
        {

        }
    }
}
