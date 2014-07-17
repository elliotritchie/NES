// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWorkTests.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The unit of work tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using NES.Tests.Stubs;

    /// <summary>
    ///     The unit of work tests.
    /// </summary>
    public static class UnitOfWorkTests
    {
        /// <summary>
        ///     The when_committing_and_a_null_event_source_has_been_registered.
        /// </summary>
        [TestClass]
        public class When_committing_and_a_null_event_source_has_been_registered : Test
        {
            #region Fields

            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();

            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();

            private UnitOfWork _unitOfWork;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_not_call_event_source_mapper.
            /// </summary>
            [TestMethod]
            public void Should_not_call_event_source_mapper()
            {
                this._eventSourceMapper.Verify(m => m.Set(this._commandContext, It.IsAny<IEventSource>()), Times.Never());
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._unitOfWork = new UnitOfWork(this._commandContextProvider.Object, this._eventSourceMapper.Object);

                this._commandContextProvider.Setup(p => p.Get()).Returns(this._commandContext);
                this._unitOfWork.Register<AggregateStub>(null);
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._unitOfWork.Commit();
            }

            #endregion
        }

        /// <summary>
        ///     The when_committing_and_an_event_source_has_been_registered.
        /// </summary>
        [TestClass]
        public class When_committing_and_an_event_source_has_been_registered : Test
        {
            #region Fields

            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();

            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();

            private IEventSource _aggregate;

            private UnitOfWork _unitOfWork;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_set_aggregate_in_event_source_mapper_once.
            /// </summary>
            [TestMethod]
            public void Should_set_aggregate_in_event_source_mapper_once()
            {
                this._eventSourceMapper.Verify(m => m.Set(this._commandContext, this._aggregate), Times.Once());
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._unitOfWork = new UnitOfWork(this._commandContextProvider.Object, this._eventSourceMapper.Object);
                this._aggregate = new AggregateStub();

                this._commandContextProvider.Setup(p => p.Get()).Returns(this._commandContext);
                this._unitOfWork.Register(this._aggregate);
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._unitOfWork.Commit();
            }

            #endregion
        }

        /// <summary>
        ///     The when_committing_and_an_event_source_has_been_registered_more_than_once.
        /// </summary>
        [TestClass]
        public class When_committing_and_an_event_source_has_been_registered_more_than_once : Test
        {
            #region Fields

            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();

            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();

            private IEventSource _aggregate;

            private UnitOfWork _unitOfWork;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_set_aggregate_in_event_source_mapper_once.
            /// </summary>
            [TestMethod]
            public void Should_set_aggregate_in_event_source_mapper_once()
            {
                this._eventSourceMapper.Verify(m => m.Set(this._commandContext, this._aggregate), Times.Once());
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._unitOfWork = new UnitOfWork(this._commandContextProvider.Object, this._eventSourceMapper.Object);
                this._aggregate = new AggregateStub();

                this._commandContextProvider.Setup(p => p.Get()).Returns(this._commandContext);
                this._unitOfWork.Register(this._aggregate);
                this._unitOfWork.Register(this._aggregate);
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._unitOfWork.Commit();
            }

            #endregion
        }

        /// <summary>
        ///     The when_committing_and_event_sources_have_been_registered.
        /// </summary>
        [TestClass]
        public class When_committing_and_event_sources_have_been_registered : Test
        {
            #region Fields

            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();

            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();

            private IEventSource _aggregate1;

            private IEventSource _aggregate2;

            private UnitOfWork _unitOfWork;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_set_aggregates_in_event_source_mapper_once.
            /// </summary>
            [TestMethod]
            public void Should_set_aggregates_in_event_source_mapper_once()
            {
                this._eventSourceMapper.Verify(m => m.Set(this._commandContext, this._aggregate1), Times.Once());
                this._eventSourceMapper.Verify(m => m.Set(this._commandContext, this._aggregate2), Times.Once());
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._unitOfWork = new UnitOfWork(this._commandContextProvider.Object, this._eventSourceMapper.Object);
                this._aggregate1 = new AggregateStub();
                this._aggregate2 = new AggregateStub();

                this._commandContextProvider.Setup(p => p.Get()).Returns(this._commandContext);
                this._unitOfWork.Register(this._aggregate1);
                this._unitOfWork.Register(this._aggregate2);
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._unitOfWork.Commit();
            }

            #endregion
        }

        /// <summary>
        ///     The when_getting_an_event_source_more_than_once.
        /// </summary>
        [TestClass]
        public class When_getting_an_event_source_more_than_once : Test
        {
            #region Fields

            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();

            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();

            private readonly Guid _id = Guid.NewGuid();

            private AggregateStub _aggregate;

            private AggregateStub _returnedAggregate;

            private UnitOfWork _unitOfWork;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_get_aggregate_from_event_source_mapper_once.
            /// </summary>
            [TestMethod]
            public void Should_get_aggregate_from_event_source_mapper_once()
            {
                this._eventSourceMapper.Verify(m => m.Get<AggregateStub>(BucketSupport.DefaultBucketId, this._id), Times.Once());
            }

            /// <summary>
            ///     The should_get_command_context_once.
            /// </summary>
            [TestMethod]
            public void Should_get_command_context_once()
            {
                this._commandContextProvider.Verify(p => p.Get(), Times.Once());
            }

            /// <summary>
            ///     The should_return_aggregate.
            /// </summary>
            [TestMethod]
            public void Should_return_aggregate()
            {
                Assert.AreSame(this._aggregate, this._returnedAggregate);
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._unitOfWork = new UnitOfWork(this._commandContextProvider.Object, this._eventSourceMapper.Object);
                this._aggregate = new AggregateStub(this._id);

                this._commandContextProvider.Setup(p => p.Get()).Returns(this._commandContext);
                this._eventSourceMapper.Setup(m => m.Get<AggregateStub>(BucketSupport.DefaultBucketId, this._id)).Returns(this._aggregate);
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._returnedAggregate = this._unitOfWork.Get<AggregateStub>(BucketSupport.DefaultBucketId, this._id);
                this._returnedAggregate = this._unitOfWork.Get<AggregateStub>(BucketSupport.DefaultBucketId, this._id);
            }

            #endregion
        }

        /// <summary>
        ///     The when_getting_an_event_source_once.
        /// </summary>
        [TestClass]
        public class When_getting_an_event_source_once : Test
        {
            #region Fields

            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();

            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();

            private readonly Guid _id = Guid.NewGuid();

            private AggregateStub _aggregate;

            private AggregateStub _returnedAggregate;

            private UnitOfWork _unitOfWork;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_get_aggregate_from_event_source_mapper.
            /// </summary>
            [TestMethod]
            public void Should_get_aggregate_from_event_source_mapper()
            {
                this._eventSourceMapper.Verify(m => m.Get<AggregateStub>(BucketSupport.DefaultBucketId, this._id));
            }

            /// <summary>
            ///     The should_get_command_context.
            /// </summary>
            [TestMethod]
            public void Should_get_command_context()
            {
                this._commandContextProvider.Verify(p => p.Get());
            }

            /// <summary>
            ///     The should_return_aggregate.
            /// </summary>
            [TestMethod]
            public void Should_return_aggregate()
            {
                Assert.AreSame(this._aggregate, this._returnedAggregate);
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._unitOfWork = new UnitOfWork(this._commandContextProvider.Object, this._eventSourceMapper.Object);
                this._aggregate = new AggregateStub(this._id);

                this._commandContextProvider.Setup(p => p.Get()).Returns(this._commandContext);
                this._eventSourceMapper.Setup(m => m.Get<AggregateStub>(BucketSupport.DefaultBucketId, this._id)).Returns(this._aggregate);
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._returnedAggregate = this._unitOfWork.Get<AggregateStub>(BucketSupport.DefaultBucketId, this._id);
            }

            #endregion
        }

        /// <summary>
        ///     The when_registering_a_null_event_source.
        /// </summary>
        [TestClass]
        public class When_registering_a_null_event_source : Test
        {
            #region Fields

            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();

            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();

            private UnitOfWork _unitOfWork;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_get_command_context.
            /// </summary>
            [TestMethod]
            public void Should_get_command_context()
            {
                this._commandContextProvider.Verify(p => p.Get());
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._unitOfWork = new UnitOfWork(this._commandContextProvider.Object, this._eventSourceMapper.Object);

                this._commandContextProvider.Setup(p => p.Get()).Returns(this._commandContext);
                this._unitOfWork.Register<AggregateStub>(null);
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._unitOfWork.Commit();
            }

            #endregion
        }

        /// <summary>
        ///     The when_trying_to_get_an_event_source_that_doesnt_exist.
        /// </summary>
        [TestClass]
        public class When_trying_to_get_an_event_source_that_doesnt_exist : Test
        {
            #region Fields

            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();

            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();

            private readonly Guid _id = Guid.NewGuid();

            private AggregateStub _returnedAggregate;

            private UnitOfWork _unitOfWork;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_get_aggregate_from_event_source_mapper.
            /// </summary>
            [TestMethod]
            public void Should_get_aggregate_from_event_source_mapper()
            {
                this._eventSourceMapper.Verify(m => m.Get<AggregateStub>(BucketSupport.DefaultBucketId, this._id));
            }

            /// <summary>
            ///     The should_get_command_context_once.
            /// </summary>
            [TestMethod]
            public void Should_get_command_context_once()
            {
                this._commandContextProvider.Verify(p => p.Get(), Times.Once());
            }

            /// <summary>
            ///     The should_return_null.
            /// </summary>
            [TestMethod]
            public void Should_return_null()
            {
                Assert.IsNull(this._returnedAggregate);
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._unitOfWork = new UnitOfWork(this._commandContextProvider.Object, this._eventSourceMapper.Object);

                this._commandContextProvider.Setup(p => p.Get()).Returns(this._commandContext);
                this._eventSourceMapper.Setup(m => m.Get<AggregateStub>(BucketSupport.DefaultBucketId, this._id)).Returns<AggregateStub>(null);
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._returnedAggregate = this._unitOfWork.Get<AggregateStub>(BucketSupport.DefaultBucketId, this._id);
            }

            #endregion
        }
    }
}