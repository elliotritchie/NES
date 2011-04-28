using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NES.Tests
{
    [TestClass]
    public class DependencyInjectionContainerTests
    {
        private readonly DependencyInjectionContainer _container = new DependencyInjectionContainer();

        [TestMethod]
        public void Should_resolve_registered()
        {
            _container.Register<IFoo1>(() => new Foo());

            Assert.IsInstanceOfType(_container.Resolve<IFoo1>(), typeof(Foo));
        }

        [TestMethod]
        public void Should_return_null_when_dependency_is_not_registered()
        {
            Assert.IsNull(_container.Resolve<IFoo1>());
        }

        [TestMethod]
        public void Should_resolve_inner_dependencies()
        {
            _container.Register<IFoo1>(() => new Foo());
            _container.Register<IBar, IFoo1>((foo) => new Bar(foo));

            var result = _container.Resolve<IBar>();
            Assert.IsInstanceOfType(result, typeof(Bar));
            Assert.IsInstanceOfType(result.Foo1, typeof(Foo));
        }

        [TestMethod]
        public void Should_resolve_inner_dependencies_with_two_arguments()
        {
            _container.Register<IFoo1>(() => new Foo());
            _container.Register<IFoo2>(() => new Foo());
            _container.Register<IBar, IFoo1, IFoo2>((foo1, foo2) => new Bar(foo1, foo2));

            var result = _container.Resolve<IBar>();

            Assert.IsInstanceOfType(result, typeof(Bar));
            Assert.IsInstanceOfType(result.Foo1, typeof(Foo));
            Assert.IsNotNull(result.Foo1);
            Assert.IsInstanceOfType(result.Foo2, typeof(Foo));
            Assert.IsNotNull(result.Foo2);
        }

        [TestMethod]
        public void Should_resolve_inner_dependencies_with_three_arguments()
        {
            _container.Register<IFoo1>(() => new Foo());
            _container.Register<IFoo2>(() => new Foo());
            _container.Register<IFoo3>(() => new Foo());

            _container.Register<IBar, IFoo1, IFoo2, IFoo3>((foo1, foo2, foo3) => new Bar(foo1, foo2, foo3));

            var result = _container.Resolve<IBar>();

            Assert.IsInstanceOfType(result, typeof(Bar));
            Assert.IsInstanceOfType(result.Foo1, typeof(Foo));
            Assert.IsNotNull(result.Foo1);
            Assert.IsInstanceOfType(result.Foo2, typeof(Foo));
            Assert.IsNotNull(result.Foo2);
            Assert.IsInstanceOfType(result.Foo3, typeof(Foo));
            Assert.IsNotNull(result.Foo3);
        }

        [TestMethod]
        public void Should_resolve_inner_dependencies_with_four_arguments()
        {
            _container.Register<IFoo1>(() => new Foo());
            _container.Register<IFoo2>(() => new Foo());
            _container.Register<IFoo3>(() => new Foo());
            _container.Register<IFoo4>(() => new Foo());

            _container.Register<IBar, IFoo1, IFoo2, IFoo3, IFoo4>((foo1, foo2, foo3, foo4) => new Bar(foo1, foo2, foo3, foo4));

            var result = _container.Resolve<IBar>();

            Assert.IsInstanceOfType(result, typeof(Bar));
            Assert.IsInstanceOfType(result.Foo1, typeof(Foo));
            Assert.IsNotNull(result.Foo1);
            Assert.IsInstanceOfType(result.Foo2, typeof(Foo));
            Assert.IsNotNull(result.Foo2);
            Assert.IsInstanceOfType(result.Foo3, typeof(Foo));
            Assert.IsNotNull(result.Foo3);
            Assert.IsInstanceOfType(result.Foo4, typeof(Foo));
            Assert.IsNotNull(result.Foo4);
        }

        [TestMethod]
        public void Should_allow_explicit_depdendency_resolution()
        {
            _container.Register<IFoo1>(() => new Foo());

            _container.Register<IBar>(c => new Bar(c.Resolve<IFoo1>()));

            var result = _container.Resolve<IBar>();

            Assert.IsInstanceOfType(result, typeof(Bar));
            Assert.IsInstanceOfType(result.Foo1, typeof(Foo));
            Assert.IsNotNull(result.Foo1);
        }

        interface IFoo1
        {
        }

        interface IFoo2
        {
        }

        interface IFoo3
        {
        }

        interface IFoo4
        {
        }

        class Foo : IFoo1, IFoo2, IFoo3, IFoo4
        {
        }

        interface IBar
        {
            IFoo1 Foo1 { get; }
            IFoo2 Foo2 { get; }
            IFoo3 Foo3 { get; }
            IFoo4 Foo4 { get; }
        }

        class Bar : IBar
        {
            public IFoo1 Foo1 { get; private set; }
            public IFoo2 Foo2 { get; private set; }
            public IFoo3 Foo3 { get; private set; }
            public IFoo4 Foo4 { get; private set; }
            
            public Bar(IFoo1 foo1, IFoo2 foo2, IFoo3 foo3, IFoo4 foo4)
            {
                Foo1 = foo1;
                Foo2 = foo2;
                Foo3 = foo3;
                Foo4 = foo4;
            }

            public Bar(IFoo1 foo1, IFoo2 foo2, IFoo3 foo3)
                : this(foo1, foo2, foo3, null)
            {
            }

            public Bar(IFoo1 foo1, IFoo2 foo2)
                : this(foo1, foo2, null)
            {
                Foo1 = foo1;
            }

            public Bar(IFoo1 foo1)
                : this(foo1, null, null)
            {
            }
        }
    }
}