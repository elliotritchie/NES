// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Car.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The car.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NES.NEventStore.Tests.TestDomain
{
    using System;

    /// <summary>
    /// The car.
    /// </summary>
    public class Car : AggregateBase
    {
        #region Fields

        private string name;

        private double price;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Car"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="bucketId">
        /// The bucket id.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Id and name must not be null
        /// </exception>
        public Car(Guid id, string name, string bucketId = null)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(name);
            }

            this.Apply<ICarCreated>(
                e =>
                    {
                        e.Id = id;
                        e.Name = name;
                        e.BucketId = bucketId;
                    });
        }

        private Car()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The change price.
        /// </summary>
        /// <param name="newPrice">
        /// The new price.
        /// </param>
        /// <exception cref="Exception">
        /// Price must be >= 0
        /// </exception>
        public void ChangePrice(double newPrice)
        {
            if (newPrice < 0)
            {
                throw new Exception(string.Format("Price {0} is invalid. Must be >= 0", newPrice));
            }

            this.Apply<ICarPriceChanged>(
                e =>
                    {
                        e.Id = this.Id;
                        e.BucketId = this.BucketId;
                        e.Price = newPrice;
                    });
        }

        #endregion

        #region Methods

        private void Handle(ICarCreated @event)
        {
            this.Id = @event.Id;
            this.BucketId = @event.BucketId;
            this.name = @event.Name;
        }

        private void Handle(ICarPriceChanged @event)
        {
            this.price = @event.Price;
        }

        #endregion
    }
}