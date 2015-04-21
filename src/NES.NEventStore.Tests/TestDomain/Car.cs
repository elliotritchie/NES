using System;

namespace NES.NEventStore.Tests.TestDomain
{
    public class Car : AggregateBase<Guid>
    {
        private string name;

        private double price;

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
    }
}