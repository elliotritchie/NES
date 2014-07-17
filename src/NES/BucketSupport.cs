// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BucketSupport.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The bucket support.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     The bucket support.
    /// </summary>
    public class BucketSupport
    {
        #region Static Fields

        private static string defaultBucketId = "default";

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the default bucket id.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Bucket must not be empty
        /// </exception>
        public static string DefaultBucketId
        {
            get
            {
                return defaultBucketId;
            }

            set
            {
                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(DefaultBucketId);
                }

                defaultBucketId = value;
            }
        }

        #endregion
    }
}