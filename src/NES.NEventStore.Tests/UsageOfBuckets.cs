// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsageOfBuckets.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The usage of buckets.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NES.NEventStore.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NES.NEventStore.Tests.TestDomain;

    /// <summary>
    /// The usage of buckets.
    /// </summary>
    [TestClass]
    public class UsageOfBuckets : TestIntegration
    {
        #region Constants

        private const string TestBucketId = "MyTestBucket";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The writ and read using bucket and one unit of work.
        /// </summary>
        [TestMethod]
        public void WritAndReadUsingBucketAndOneUnitOfWork()
        {
            var car = this.WritAndReadUsingOneUnitOfWork(TestBucketId);
            Assert.IsTrue(car.BucketId == TestBucketId);
        }

        /// <summary>
        /// The write and read using bucket.
        /// </summary>
        [TestMethod]
        public void WriteAndReadUsingBucket()
        {
            var car = this.WriteAndReadCar(TestBucketId);
            Assert.IsTrue(car.BucketId == TestBucketId);
        }

        /// <summary>
        /// The write without bucket and read without bucket.
        /// </summary>
        [TestMethod]
        public void WriteWithoutBucketAndReadWithoutBucket()
        {
            this.WriteAndReadCar();
        }

        /// <summary>
        /// The write without bucket and read without bucket only one unit of work.
        /// </summary>
        [TestMethod]
        public void WriteWithoutBucketAndReadWithoutBucketOnlyOneUnitOfWork()
        {
            this.WritAndReadUsingOneUnitOfWork();
        }

        #endregion

        #region Methods

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

        #endregion
    }
}