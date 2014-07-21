using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NES.Tests.Stubs;

namespace NES.Tests
{
    public static class UnitOfWorkTests
    {
        [TestClass]
        public class When_committing_and_a_null_event_source_has_been_registered : Test
        {
            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();

            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();

            private UnitOfWork _unitOfWork;

            [TestMethod]
            public void Should_not_call_event_source_mapper()
            {
                _eventSourceMapper.Verify(m => m.Set(this._commandContext, It.IsAny<IEventSource>()), Times.Never());
            }

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(this._commandContextProvider.Object, _eventSourceMapper.Object);

                _commandContextProvider.Setup(p => p.Get()).Returns(this._commandContext);
                _unitOfWork.Register<AggregateStub>(null);
            }

            protected override void Event()
            {
                _unitOfWork.Commit();
            }
        }

        [TestClass]
        public class When_committing_and_an_event_source_has_been_registered : Test
        {
            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();

            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();

            private IEventSource _aggregate;

            private UnitOfWork _unitOfWork;

            [TestMethod]
            public void Should_set_aggregate_in_event_source_mapper_once()
            {
                _eventSourceMapper.Verify(m => m.Set(this._commandContext, _aggregate), Times.Once());
            }

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(this._commandContextProvider.Object, _eventSourceMapper.Object);
                _aggregate = new AggregateStub();

                _commandContextProvider.Setup(p => p.Get()).Returns(this._commandContext);
                _unitOfWork.Register(this._aggregate);
            }

            protected override void Event()
            {
                _unitOfWork.Commit();
            }
        }

        [TestClass]
        public class When_committing_and_an_event_source_has_been_registered_more_than_once : Test
        {
            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();

            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();

            private IEventSource _aggregate;

            private UnitOfWork _unitOfWork;

            [TestMethod]
            public void Should_set_aggregate_in_event_source_mapper_once()
            {
                _eventSourceMapper.Verify(m => m.Set(this._commandContext, _aggregate), Times.Once());
            }

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(this._commandContextProvider.Object, _eventSourceMapper.Object);
                _aggregate = new AggregateStub();

                _commandContextProvider.Setup(p => p.Get()).Returns(this._commandContext);
                _unitOfWork.Register(this._aggregate);
                _unitOfWork.Register(this._aggregate);
            }

            protected override void Event()
            {
                _unitOfWork.Commit();
            }
        }

        [TestClass]
        public class When_committing_and_event_sources_have_been_registered : Test
        {
            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();

            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();

            private IEventSource _aggregate1;

            private IEventSource _aggregate2;

            private UnitOfWork _unitOfWork;

            [TestMethod]
            public void Should_set_aggregates_in_event_source_mapper_once()
            {
                _eventSourceMapper.Verify(m => m.Set(this._commandContext, _aggregate1), Times.Once());
                _eventSourceMapper.Verify(m => m.Set(this._commandContext, _aggregate2), Times.Once());
            }

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(this._commandContextProvider.Object, _eventSourceMapper.Object);
                _aggregate1 = new AggregateStub();
                _aggregate2 = new AggregateStub();

                _commandContextProvider.Setup(p => p.Get()).Returns(this._commandContext);
                _unitOfWork.Register(this._aggregate1);
                _unitOfWork.Register(this._aggregate2);
            }

            protected override void Event()
            {
                _unitOfWork.Commit();
            }
        }

        [TestClass]
        public class When_getting_an_event_source_more_than_once : Test
        {
            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();

            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();

            private readonly Guid _id = Guid.NewGuid();

            private AggregateStub _aggregate;

            private AggregateStub _returnedAggregate;

            private UnitOfWork _unitOfWork;

            [TestMethod]
            public void Should_get_aggregate_from_event_source_mapper_once()
            {
                _eventSourceMapper.Verify(m => m.Get<AggregateStub>(BucketSupport.DefaultBucketId, _id), Times.Once());
            }

            [TestMethod]
            public void Should_get_command_context_once()
            {
                _commandContextProvider.Verify(p => p.Get(), Times.Once());
            }

            [TestMethod]
            public void Should_return_aggregate()
            {
                Assert.AreSame(this._aggregate, _returnedAggregate);
            }

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(this._commandContextProvider.Object, _eventSourceMapper.Object);
                _aggregate = new AggregateStub(this._id);

                _commandContextProvider.Setup(p => p.Get()).Returns(this._commandContext);
                _eventSourceMapper.Setup(m => m.Get<AggregateStub>(BucketSupport.DefaultBucketId, _id)).Returns(this._aggregate);
            }

            protected override void Event()
            {
                _returnedAggregate = _unitOfWork.Get<AggregateStub>(BucketSupport.DefaultBucketId, _id);
                _returnedAggregate = _unitOfWork.Get<AggregateStub>(BucketSupport.DefaultBucketId, _id);
            }
        }

        [TestClass]
        public class When_getting_an_event_source_once : Test
        {
            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();

            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();

            private readonly Guid _id = Guid.NewGuid();

            private AggregateStub _aggregate;

            private AggregateStub _returnedAggregate;

            private UnitOfWork _unitOfWork;

            [TestMethod]
            public void Should_get_aggregate_from_event_source_mapper()
            {
                _eventSourceMapper.Verify(m => m.Get<AggregateStub>(BucketSupport.DefaultBucketId, _id));
            }

            [TestMethod]
            public void Should_get_command_context()
            {
                _commandContextProvider.Verify(p => p.Get());
            }

            [TestMethod]
            public void Should_return_aggregate()
            {
                Assert.AreSame(this._aggregate, _returnedAggregate);
            }

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(this._commandContextProvider.Object, _eventSourceMapper.Object);
                _aggregate = new AggregateStub(this._id);

                _commandContextProvider.Setup(p => p.Get()).Returns(this._commandContext);
                _eventSourceMapper.Setup(m => m.Get<AggregateStub>(BucketSupport.DefaultBucketId, _id)).Returns(this._aggregate);
            }

            protected override void Event()
            {
                _returnedAggregate = _unitOfWork.Get<AggregateStub>(BucketSupport.DefaultBucketId, _id);
            }
        }

        [TestClass]
        public class When_registering_a_null_event_source : Test
        {
            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();

            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();

            private UnitOfWork _unitOfWork;

            [TestMethod]
            public void Should_get_command_context()
            {
                _commandContextProvider.Verify(p => p.Get());
            }

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(this._commandContextProvider.Object, _eventSourceMapper.Object);

                _commandContextProvider.Setup(p => p.Get()).Returns(this._commandContext);
                _unitOfWork.Register<AggregateStub>(null);
            }

            protected override void Event()
            {
                _unitOfWork.Commit();
            }
        }

        [TestClass]
        public class When_trying_to_get_an_event_source_that_doesnt_exist : Test
        {
            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();

            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();

            private readonly Guid _id = Guid.NewGuid();

            private AggregateStub _returnedAggregate;

            private UnitOfWork _unitOfWork;

            [TestMethod]
            public void Should_get_aggregate_from_event_source_mapper()
            {
                _eventSourceMapper.Verify(m => m.Get<AggregateStub>(BucketSupport.DefaultBucketId, _id));
            }

            [TestMethod]
            public void Should_get_command_context_once()
            {
                _commandContextProvider.Verify(p => p.Get(), Times.Once());
            }

            [TestMethod]
            public void Should_return_null()
            {
                Assert.IsNull(this._returnedAggregate);
            }

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(this._commandContextProvider.Object, _eventSourceMapper.Object);

                _commandContextProvider.Setup(p => p.Get()).Returns(this._commandContext);
                _eventSourceMapper.Setup(m => m.Get<AggregateStub>(BucketSupport.DefaultBucketId, _id)).Returns<AggregateStub>(null);
            }

            protected override void Event()
            {
                _returnedAggregate = _unitOfWork.Get<AggregateStub>(BucketSupport.DefaultBucketId, _id);
            }
        }
    }
}