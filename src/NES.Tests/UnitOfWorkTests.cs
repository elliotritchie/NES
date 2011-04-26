using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NES.Tests.Stubs;

namespace NES.Tests
{
    public static class UnitOfWorkTests
    {
        [TestClass]
        public class When_getting_an_event_source_once : Test
        {
            private readonly Guid _id = Guid.NewGuid();
            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();
            private UnitOfWork _unitOfWork;
            private AggregateStub _aggregate;
            private AggregateStub _returnedAggregate;

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(_eventSourceMapper.Object);
                _aggregate = new AggregateStub(_id);

                _eventSourceMapper.Setup(m => m.Get<AggregateStub>(_id)).Returns(_aggregate);
            }

            protected override void Event()
            {
                _returnedAggregate = _unitOfWork.Get<AggregateStub>(_id);
            }

            [TestMethod]
            public void Should_get_aggregate_from_event_source_mapper_once()
            {
                _eventSourceMapper.Verify(m => m.Get<AggregateStub>(_id), Times.Once());

                Assert.AreSame(_aggregate, _returnedAggregate);
            }
        }

        [TestClass]
        public class When_getting_an_event_source_more_than_once : Test
        {
            private readonly Guid _id = Guid.NewGuid();
            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();
            private UnitOfWork _unitOfWork;
            private AggregateStub _aggregate;
            private AggregateStub _returnedAggregate;

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(_eventSourceMapper.Object);
                _aggregate = new AggregateStub(_id);

                _eventSourceMapper.Setup(m => m.Get<AggregateStub>(_id)).Returns(_aggregate);
            }

            protected override void Event()
            {
                _returnedAggregate = _unitOfWork.Get<AggregateStub>(_id);
                _returnedAggregate = _unitOfWork.Get<AggregateStub>(_id);
            }

            [TestMethod]
            public void Should_get_aggregate_from_event_source_mapper_once()
            {
                _eventSourceMapper.Verify(m => m.Get<AggregateStub>(_id), Times.Once());

                Assert.AreSame(_aggregate, _returnedAggregate);
            }
        }

        [TestClass]
        public class When_committing_and_an_event_source_has_been_registered : Test
        {
            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();
            private UnitOfWork _unitOfWork;
            private IEventSource _aggregate;

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(_eventSourceMapper.Object);
                _aggregate = new AggregateStub();

                _unitOfWork.Register(_aggregate);
            }

            protected override void Event()
            {
                _unitOfWork.Commit();
            }

            [TestMethod]
            public void Should_set_aggregate_in_event_source_mapper_once()
            {
                _eventSourceMapper.Verify(m => m.Set(_aggregate), Times.Once());
            }
        }

        [TestClass]
        public class When_committing_and_an_event_source_has_been_registered_more_than_once : Test
        {
            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();
            private UnitOfWork _unitOfWork;
            private IEventSource _aggregate;

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(_eventSourceMapper.Object);
                _aggregate = new AggregateStub();

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
                _eventSourceMapper.Verify(m => m.Set(_aggregate), Times.Once());
            }
        }

        [TestClass]
        public class When_committing_and_event_sources_have_been_registered : Test
        {
            private readonly Mock<IEventSourceMapper> _eventSourceMapper = new Mock<IEventSourceMapper>();
            private UnitOfWork _unitOfWork;
            private IEventSource _aggregate1;
            private IEventSource _aggregate2;

            protected override void Context()
            {
                _unitOfWork = new UnitOfWork(_eventSourceMapper.Object);
                _aggregate1 = new AggregateStub();
                _aggregate2 = new AggregateStub();

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
                _eventSourceMapper.Verify(m => m.Set(_aggregate1), Times.Once());
                _eventSourceMapper.Verify(m => m.Set(_aggregate2), Times.Once());
            }
        }
    }
}