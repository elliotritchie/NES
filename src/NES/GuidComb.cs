// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuidComb.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   Based on the GuidCombGenerator implementation in NHibernate
//   https://nhibernate.svn.sourceforge.net/svnroot/nhibernate/trunk/nhibernate/src/NHibernate/Id/GuidCombGenerator.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     Based on the GuidCombGenerator implementation in NHibernate
    ///     https://nhibernate.svn.sourceforge.net/svnroot/nhibernate/trunk/nhibernate/src/NHibernate/Id/GuidCombGenerator.cs
    /// </summary>
    public static class GuidComb
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The new guid comb.
        /// </summary>
        /// <returns>
        ///     The <see cref="Guid" />.
        /// </returns>
        public static Guid NewGuidComb()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();

            var baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;

            // Get the days and milliseconds which will be used to build the byte string 
            var days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = now.TimeOfDay;

            // Convert to a byte array 
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }

        #endregion
    }
}