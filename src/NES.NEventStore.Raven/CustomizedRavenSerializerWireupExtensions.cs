// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomizedRavenSerializerWireupExtensions.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The customized raven serializer wireup extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NEventStore.Raven
{
    /// <summary>
    ///     The customized raven serializer wireup extensions.
    /// </summary>
    public static class CustomizedRavenSerializerWireupExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// The using customized raven serializer.
        /// </summary>
        /// <param name="wireup">
        /// The wireup.
        /// </param>
        /// <returns>
        /// The <see cref="CustomizedRavenSerializerWireup"/>.
        /// </returns>
        public static CustomizedRavenSerializerWireup UsingCustomizedRavenSerializer(this NESWireup wireup)
        {
            return new CustomizedRavenSerializerWireup(wireup);
        }

        #endregion
    }
}