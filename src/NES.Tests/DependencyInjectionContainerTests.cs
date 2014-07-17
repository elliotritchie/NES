// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyInjectionContainerTests.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The dependency injection container tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     The dependency injection container tests.
    /// </summary>
    [TestClass]
    public class DependencyInjectionContainerTests
    {
        #region Fields

        private readonly DependencyInjectionContainer _container = new DependencyInjectionContainer();

        #endregion

        #region Interfaces

        private interface IBar
        {
            #region Public Properties

            /// <summary>
            ///     Gets the foo 1.
            /// </summary>
            IFoo1 Foo1 { get; }

            /// <summary>
            ///     Gets the foo 2.
            /// </summary>
            IFoo2 Foo2 { get; }

            /// <summary>
            ///     Gets the foo 3.
            /// </summary>
            IFoo3 Foo3 { get; }

            /// <summary>
            ///     Gets the foo 4.
            /// </summary>
            IFoo4 Foo4 { get; }

            #endregion
        }

        private interface IFoo1
        {
        }

        private interface IFoo2
        {
        }

        private interface IFoo3
        {
        }

        private interface IFoo4
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The should_allow_explicit_depdendency_resolution.
        /// </summary>
        [TestMethod]
        public void Should_allow_explicit_depdendency_resolution()
        {
            this._container.Register<IFoo1>(() => new Foo());

            this._container.Register<IBar>(c => new Bar(c.Resolve<IFoo1>()));

            var result = this._container.Resolve<IBar>();

            Assert.IsInstanceOfType(result, typeof(Bar));
            Assert.IsInstanceOfType(result.Foo1, typeof(Foo));
            Assert.IsNotNull(result.Foo1);
        }

        /// <summary>
        ///     The should_resolve_inner_dependencies.
        /// </summary>
        [TestMethod]
        public void Should_resolve_inner_dependencies()
        {
            this._container.Register<IFoo1>(() => new Foo());
            this._container.Register<IBar, IFoo1>((foo) => new Bar(foo));

            var result = this._container.Resolve<IBar>();
            Assert.IsInstanceOfType(result, typeof(Bar));
            Assert.IsInstanceOfType(result.Foo1, typeof(Foo));
        }

        /// <summary>
        ///     The should_resolve_inner_dependencies_with_four_arguments.
        /// </summary>
        [TestMethod]
        public void Should_resolve_inner_dependencies_with_four_arguments()
        {
            this._container.Register<IFoo1>(() => new Foo());
            this._container.Register<IFoo2>(() => new Foo());
            this._container.Register<IFoo3>(() => new Foo());
            this._container.Register<IFoo4>(() => new Foo());

            this._container.Register<IBar, IFoo1, IFoo2, IFoo3, IFoo4>((foo1, foo2, foo3, foo4) => new Bar(foo1, foo2, foo3, foo4));

            var result = this._container.Resolve<IBar>();

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

        /// <summary>
        ///     The should_resolve_inner_dependencies_with_three_arguments.
        /// </summary>
        [TestMethod]
        public void Should_resolve_inner_dependencies_with_three_arguments()
        {
            this._container.Register<IFoo1>(() => new Foo());
            this._container.Register<IFoo2>(() => new Foo());
            this._container.Register<IFoo3>(() => new Foo());

            this._container.Register<IBar, IFoo1, IFoo2, IFoo3>((foo1, foo2, foo3) => new Bar(foo1, foo2, foo3));

            var result = this._container.Resolve<IBar>();

            Assert.IsInstanceOfType(result, typeof(Bar));
            Assert.IsInstanceOfType(result.Foo1, typeof(Foo));
            Assert.IsNotNull(result.Foo1);
            Assert.IsInstanceOfType(result.Foo2, typeof(Foo));
            Assert.IsNotNull(result.Foo2);
            Assert.IsInstanceOfType(result.Foo3, typeof(Foo));
            Assert.IsNotNull(result.Foo3);
        }

        /// <summary>
        ///     The should_resolve_inner_dependencies_with_two_arguments.
        /// </summary>
        [TestMethod]
        public void Should_resolve_inner_dependencies_with_two_arguments()
        {
            this._container.Register<IFoo1>(() => new Foo());
            this._container.Register<IFoo2>(() => new Foo());
            this._container.Register<IBar, IFoo1, IFoo2>((foo1, foo2) => new Bar(foo1, foo2));

            var result = this._container.Resolve<IBar>();

            Assert.IsInstanceOfType(result, typeof(Bar));
            Assert.IsInstanceOfType(result.Foo1, typeof(Foo));
            Assert.IsNotNull(result.Foo1);
            Assert.IsInstanceOfType(result.Foo2, typeof(Foo));
            Assert.IsNotNull(result.Foo2);
        }

        /// <summary>
        ///     The should_resolve_registered.
        /// </summary>
        [TestMethod]
        public void Should_resolve_registered()
        {
            this._container.Register<IFoo1>(() => new Foo());

            Assert.IsInstanceOfType(this._container.Resolve<IFoo1>(), typeof(Foo));
        }

        /// <summary>
        ///     The should_return_null_when_dependency_is_not_registered.
        /// </summary>
        [TestMethod]
        public void Should_return_null_when_dependency_is_not_registered()
        {
            Assert.IsNull(this._container.Resolve<IFoo1>());
        }

        #endregion

        private class Bar : IBar
        {
            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="Bar"/> class.
            /// </summary>
            /// <param name="foo1">
            /// The foo 1.
            /// </param>
            /// <param name="foo2">
            /// The foo 2.
            /// </param>
            /// <param name="foo3">
            /// The foo 3.
            /// </param>
            /// <param name="foo4">
            /// The foo 4.
            /// </param>
            public Bar(IFoo1 foo1, IFoo2 foo2, IFoo3 foo3, IFoo4 foo4)
            {
                this.Foo1 = foo1;
                this.Foo2 = foo2;
                this.Foo3 = foo3;
                this.Foo4 = foo4;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Bar"/> class.
            /// </summary>
            /// <param name="foo1">
            /// The foo 1.
            /// </param>
            /// <param name="foo2">
            /// The foo 2.
            /// </param>
            /// <param name="foo3">
            /// The foo 3.
            /// </param>
            public Bar(IFoo1 foo1, IFoo2 foo2, IFoo3 foo3)
                : this(foo1, foo2, foo3, null)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Bar"/> class.
            /// </summary>
            /// <param name="foo1">
            /// The foo 1.
            /// </param>
            /// <param name="foo2">
            /// The foo 2.
            /// </param>
            public Bar(IFoo1 foo1, IFoo2 foo2)
                : this(foo1, foo2, null)
            {
                this.Foo1 = foo1;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Bar"/> class.
            /// </summary>
            /// <param name="foo1">
            /// The foo 1.
            /// </param>
            public Bar(IFoo1 foo1)
                : this(foo1, null, null)
            {
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets the foo 1.
            /// </summary>
            public IFoo1 Foo1 { get; private set; }

            /// <summary>
            ///     Gets the foo 2.
            /// </summary>
            public IFoo2 Foo2 { get; private set; }

            /// <summary>
            ///     Gets the foo 3.
            /// </summary>
            public IFoo3 Foo3 { get; private set; }

            /// <summary>
            ///     Gets the foo 4.
            /// </summary>
            public IFoo4 Foo4 { get; private set; }

            #endregion
        }

        private class Foo : IFoo1, IFoo2, IFoo3, IFoo4
        {
        }
    }
}