// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryTests.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The repository tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    ///     The repository tests.
    /// </summary>
    public static class RepositoryTests
    {
        /// <summary>
        ///     The when_adding_aggregate.
        /// </summary>
        [TestClass]
        public class When_adding_aggregate : Test
        {
            #region Fields

            private readonly Mock<IEventSource> _aggregate = new Mock<IEventSource>();

            private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();

            private Repository _repository;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_register_aggregate_with_unit_of_work.
            /// </summary>
            [TestMethod]
            public void Should_register_aggregate_with_unit_of_work()
            {
                this._unitOfWork.Verify(u => u.Register(this._aggregate.Object));
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._repository = new Repository();

                UnitOfWorkFactory.Current = this._unitOfWork.Object;
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._repository.Add(this._aggregate.Object);
            }

            #endregion
        }

        /// <summary>
        ///     The when_getting_aggregate.
        /// </summary>
        [TestClass]
        public class When_getting_aggregate : Test
        {
            #region Fields

            private readonly Guid _id = GuidComb.NewGuidComb();

            private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();

            private Repository _repository;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_get_aggregate_from_unit_of_work.
            /// </summary>
            [TestMethod]
            public void Should_get_aggregate_from_unit_of_work()
            {
                this._unitOfWork.Verify(u => u.Get<IEventSource>(BucketSupport.DefaultBucketId, this._id));
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._repository = new Repository();

                UnitOfWorkFactory.Current = this._unitOfWork.Object;
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._repository.Get<IEventSource>(this._id);
            }

            #endregion
        }
    }
}