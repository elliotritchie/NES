using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NES.Contracts;

namespace NES.Tests
{
    public static class RepositoryTests
    {
        [TestClass]
        public class When_adding_aggregate : Test
        {
            private Repository _repository;
            private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();
            private readonly Mock<IEventSource> _aggregate = new Mock<IEventSource>();

            protected override void Context()
            {
                _repository = new Repository();

                UnitOfWorkFactory.Current = _unitOfWork.Object;
            }

            protected override void Event()
            {
                _repository.Add(_aggregate.Object);
            }

            [TestMethod]
            public void Should_register_aggregate_with_unit_of_work()
            {
                _unitOfWork.Verify(u => u.Register(_aggregate.Object));
            }
        }

        [TestClass]
        public class When_getting_aggregate : Test
        {
            private Repository _repository;
            private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();
            private readonly Guid _id = GuidComb.NewGuidComb();

            protected override void Context()
            {
                _repository = new Repository();

                UnitOfWorkFactory.Current = _unitOfWork.Object;
            }

            protected override void Event()
            {
                _repository.Get<IEventSource>(_id);
            }

            [TestMethod]
            public void Should_get_aggregate_from_unit_of_work()
            {
                _unitOfWork.Verify(u => u.Get<IEventSource, Guid, IMemento>(BucketSupport.DefaultBucketId, _id.ToString(), int.MaxValue));
            }
        }
    }
}