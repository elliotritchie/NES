using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace NES.Tests
{
    public static class RepositoryTests
    {
        [TestClass]
        public class When_adding_aggregate : Test
        {
            private Repository _repository;
            private readonly Mock<IDependencyInjectionContainer> _container = new Mock<IDependencyInjectionContainer>();
            private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();
            private readonly Mock<IEventSource> _aggregate = new Mock<IEventSource>();

            protected override void Context()
            {
                _container.Setup(c => c.Resolve<IUnitOfWork>()).Returns(_unitOfWork.Object);

                DI.Current = _container.Object;
                UnitOfWorkFactory.Current.Begin();
                
                _repository = new Repository();
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
            private readonly Mock<IDependencyInjectionContainer> _container = new Mock<IDependencyInjectionContainer>();
            private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();
            private readonly Guid _id = GuidComb.NewGuidComb();

            protected override void Context()
            {
                _container.Setup(c => c.Resolve<IUnitOfWork>()).Returns(_unitOfWork.Object);

                DI.Current = _container.Object;
                UnitOfWorkFactory.Current.Begin();

                _repository = new Repository();
            }

            protected override void Event()
            {
                _repository.Get<IEventSource>(_id);
            }

            [TestMethod]
            public void Should_get_aggregate_from_unit_of_work()
            {
                _unitOfWork.Verify(u => u.Get<IEventSource>(_id));
            }
        }
    }
}