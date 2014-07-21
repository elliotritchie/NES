using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NES.NEventStore.Tests.TestDomain;

namespace NES.NEventStore.Tests
{
    [TestClass]
    public class UsageOfBuckets : TestIntegration
    {
        private const string TestBucketId = "MyTestBucket";

        [TestMethod]
        public void WritAndReadUsingBucketAndOneUnitOfWork()
        {
            var car = this.WritAndReadUsingOneUnitOfWork(TestBucketId);
            Assert.IsTrue(car.BucketId == TestBucketId);
        }

        [TestMethod]
        public void WriteAndReadUsingBucket()
        {
            var car = this.WriteAndReadCar(TestBucketId);
            Assert.IsTrue(car.BucketId == TestBucketId);
        }

        [TestMethod]
        public void WriteWithoutBucketAndReadWithoutBucket()
        {
            this.WriteAndReadCar();
        }

        [TestMethod]
        public void WriteWithoutBucketAndReadWithoutBucketOnlyOneUnitOfWork()
        {
            this.WritAndReadUsingOneUnitOfWork();
        }

        private Car WritAndReadUsingOneUnitOfWork(string bucketId = null)
        {
            var car = new Car(Guid.NewGuid(), "Audi", bucketId);
            using (this.BusScope)
            {
                this.Repository.Add(car);
                var carFromRepository = this.Repository.Get<Car>(bucketId, car.Id);
                Assert.IsTrue(car.Id == carFromRepository.Id);
                return car;
            }
        }

        private Car WriteAndReadCar(string bucketId = null)
        {
            var car = new Car(Guid.NewGuid(), "Audi", bucketId);
            this.RepositoryAdd(car);
            using (this.BusScope)
            {
                var carFromRepository = this.Repository.Get<Car>(bucketId, car.Id);
                Assert.IsTrue(car.Id == carFromRepository.Id);
                return car;
            }
        }
    }
}