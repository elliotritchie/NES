using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NES.Contracts;
using NES.Tests.Stubs;

namespace NES.Tests
{
    public static class UnitOfWorkStringTests
    {
        [TestClass]
        public class When_getting_an_event_source_once : Test
        {
            private readonly Guid _id = Guid.NewGuid();
            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();
            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();
            private UnitOfWork _unitOfWork;
            private AggregateStringStub _aggregate;
            private readonly CommandContext _commandContext = new CommandContext();
            private AggregateStringStub _returnedAggregate;

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(_commandContextProvider.Object, _eventSourceMapper.Object);
                _aggregate = new AggregateStringStub(_id);

                _commandContextProvider.Setup(p => p.Get()).Returns(_commandContext);
                _eventSourceMapper.Setup(m => m.Get<AggregateStringStub, string, StringWay.IMemento>(BucketSupport.DefaultBucketId, _id.ToString())).Returns(_aggregate);
            }

            protected override void Event()
            {
                _returnedAggregate = _unitOfWork.Get<AggregateStringStub, string, StringWay.IMemento>(BucketSupport.DefaultBucketId, _id.ToString());
            }

            [TestMethod]
            public void Should_get_aggregate_from_event_source_mapper()
            {
                _eventSourceMapper.Verify(m => m.Get<AggregateStringStub, string, StringWay.IMemento>(BucketSupport.DefaultBucketId, _id.ToString()));
            }

            [TestMethod]
            public void Should_get_command_context()
            {
                _commandContextProvider.Verify(p => p.Get());
            }

            [TestMethod]
            public void Should_return_aggregate()
            {
                Assert.AreSame(_aggregate, _returnedAggregate);
            }
        }

        [TestClass]
        public class When_getting_an_event_source_more_than_once : Test
        {
            private readonly Guid _id = Guid.NewGuid();
            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();
            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();
            private UnitOfWork _unitOfWork;
            private AggregateStringStub _aggregate;
            private readonly CommandContext _commandContext = new CommandContext();
            private AggregateStringStub _returnedAggregate;

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(_commandContextProvider.Object, _eventSourceMapper.Object);
                _aggregate = new AggregateStringStub(_id);

                _commandContextProvider.Setup(p => p.Get()).Returns(_commandContext);
                _eventSourceMapper.Setup(m => m.Get<AggregateStringStub, string, StringWay.IMemento>(BucketSupport.DefaultBucketId, _id.ToString())).Returns(_aggregate);
            }

            protected override void Event()
            {
                _returnedAggregate = _unitOfWork.Get<AggregateStringStub, string, StringWay.IMemento>(BucketSupport.DefaultBucketId, _id.ToString());
                _returnedAggregate = _unitOfWork.Get<AggregateStringStub, string, StringWay.IMemento>(BucketSupport.DefaultBucketId, _id.ToString());
            }

            [TestMethod]
            public void Should_get_aggregate_from_event_source_mapper_once()
            {
                _eventSourceMapper.Verify(m => m.Get<AggregateStringStub, string, StringWay.IMemento>(BucketSupport.DefaultBucketId, _id.ToString()), Times.Once());
            }

            [TestMethod]
            public void Should_get_command_context_once()
            {
                _commandContextProvider.Verify(p => p.Get(), Times.Once());
            }

            [TestMethod]
            public void Should_return_aggregate()
            {
                Assert.AreSame(_aggregate, _returnedAggregate);
            }
        }

        [TestClass]
        public class When_trying_to_get_an_event_source_that_doesnt_exist : Test
        {
            private readonly Guid _id = Guid.NewGuid();
            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();
            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();
            private UnitOfWork _unitOfWork;
            private readonly CommandContext _commandContext = new CommandContext();
            private AggregateStringStub _returnedAggregate;

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(_commandContextProvider.Object, _eventSourceMapper.Object);

                _commandContextProvider.Setup(p => p.Get()).Returns(_commandContext);
                _eventSourceMapper.Setup(m => m.Get<AggregateStringStub, string, StringWay.IMemento>(BucketSupport.DefaultBucketId, _id.ToString())).Returns<AggregateStringStub>(null);
            }

            protected override void Event()
            {
                _returnedAggregate = _unitOfWork.Get<AggregateStringStub, string, StringWay.IMemento>(BucketSupport.DefaultBucketId, _id.ToString());
            }

            [TestMethod]
            public void Should_get_aggregate_from_event_source_mapper()
            {
                _eventSourceMapper.Verify(m => m.Get<AggregateStringStub, string, StringWay.IMemento>(BucketSupport.DefaultBucketId, _id.ToString()));
            }

            [TestMethod]
            public void Should_get_command_context_once()
            {
                _commandContextProvider.Verify(p => p.Get(), Times.Once());
            }

            [TestMethod]
            public void Should_return_null()
            {
                Assert.IsNull(_returnedAggregate);
            }
        }

        [TestClass]
        public class When_registering_a_null_event_source : Test
        {
            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();
            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();
            private UnitOfWork _unitOfWork;
            private readonly CommandContext _commandContext = new CommandContext();

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(_commandContextProvider.Object, _eventSourceMapper.Object);

                _commandContextProvider.Setup(p => p.Get()).Returns(_commandContext);
                _unitOfWork.Register<AggregateStringStub>(null);
            }

            protected override void Event()
            {
                _unitOfWork.Commit();
            }

            [TestMethod]
            public void Should_get_command_context()
            {
                _commandContextProvider.Verify(p => p.Get());
            }
        }

        [TestClass]
        public class When_committing_and_an_event_source_has_been_registered : Test
        {
            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();
            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();
            private UnitOfWork _unitOfWork;
            private StringWay.IEventSource _aggregate;
            private readonly CommandContext _commandContext = new CommandContext();

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(_commandContextProvider.Object, _eventSourceMapper.Object);
                _aggregate = new AggregateStringStub();
                _commandContextProvider.Setup(p => p.Get()).Returns(_commandContext);
                _unitOfWork.Register(_aggregate);
            }

            protected override void Event()
            {
                _unitOfWork.Commit();
            }

            [TestMethod]
            public void Should_set_aggregate_in_event_source_mapper_once()
            {
                _eventSourceMapper.Verify(m => m.Set(_commandContext, _aggregate), Times.Once());
            }
        }

        [TestClass]
        public class When_committing_and_an_event_source_has_been_registered_more_than_once : Test
        {
            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();
            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();
            private UnitOfWork _unitOfWork;
            private StringWay.IEventSource _aggregate;
            private readonly CommandContext _commandContext = new CommandContext();

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(_commandContextProvider.Object, _eventSourceMapper.Object);
                _aggregate = new AggregateStringStub();

                _commandContextProvider.Setup(p => p.Get()).Returns(_commandContext);
                _unitOfWork.Register(_aggregate);
                _unitOfWork.Register(_aggregate);
            }

            protected override void Event()
            {
                _unitOfWork.Commit();
            }

            [TestMethod]
            public void Should_set_aggregate_in_event_source_mapper_once()
            {
                _eventSourceMapper.Verify(m => m.Set(_commandContext, _aggregate), Times.Once());
            }
        }

        [TestClass]
        public class When_committing_and_event_sources_have_been_registered : Test
        {
            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();
            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();
            private UnitOfWork _unitOfWork;
            private StringWay.IEventSource _aggregate1;
            private StringWay.IEventSource _aggregate2;
            private readonly CommandContext _commandContext = new CommandContext();

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(_commandContextProvider.Object, _eventSourceMapper.Object);
                _aggregate1 = new AggregateStringStub();
                _aggregate2 = new AggregateStringStub();

                _commandContextProvider.Setup(p => p.Get()).Returns(_commandContext);
                _unitOfWork.Register(_aggregate1);
                _unitOfWork.Register(_aggregate2);
            }

            protected override void Event()
            {
                _unitOfWork.Commit();
            }

            [TestMethod]
            public void Should_set_aggregates_in_event_source_mapper_once()
            {
                _eventSourceMapper.Verify(m => m.Set(_commandContext, _aggregate1), Times.Once());
                _eventSourceMapper.Verify(m => m.Set(_commandContext, _aggregate2), Times.Once());
            }
        }

        [TestClass]
        public class When_committing_and_a_null_event_source_has_been_registered : Test
        {
            private readonly Mock<ICommandContextProvider> _commandContextProvider = new Mock<ICommandContextProvider>();
            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();
            private UnitOfWork _unitOfWork;
            private readonly CommandContext _commandContext = new CommandContext();

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(_commandContextProvider.Object, _eventSourceMapper.Object);

                _commandContextProvider.Setup(p => p.Get()).Returns(_commandContext);
                _unitOfWork.Register<AggregateStringStub>(null);
            }

            protected override void Event()
            {
                _unitOfWork.Commit();
            }

            [TestMethod]
            public void Should_not_call_event_source_mapper()
            {
                _eventSourceMapper.Verify(m => m.Set(_commandContext, It.IsAny<IEventSource>()), Times.Never());
            }
        }
    }
}