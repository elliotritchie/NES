// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AggregateStub.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The aggregate stub.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.Tests.Stubs
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     The aggregate stub.
    /// </summary>
    public class AggregateStub : AggregateBase
    {
        #region Fields

        public readonly List<IEvent> HandledEvents = new List<IEvent>();

        private string _something;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateStub"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public AggregateStub(Guid id)
        {
            this.Apply<ICreatedAggregateEvent>(e => { e.Id = id; });
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AggregateStub" /> class.
        /// </summary>
        public AggregateStub()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The do something.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public void DoSomething(string value)
        {
            this.Apply<ISomethingHappenedEvent>(e => { e.Something = value; });
        }

        #endregion

        #region Methods

        private void Handle(ICreatedAggregateEvent @event)
        {
            this.HandledEvents.Add(@event);

            this.Id = @event.Id;
        }

        private void Handle(ISomethingHappenedEvent @event)
        {
            this.HandledEvents.Add(@event);

            this._something = @event.Something;
        }

        #endregion
    }
}