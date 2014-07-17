// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogger.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The Logger interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    /// <summary>
    ///     The Logger interface.
    /// </summary>
    public interface ILogger
    {
        #region Public Methods and Operators

        /// <summary>
        /// The debug.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        void Debug(string message, params object[] args);

        /// <summary>
        /// The error.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        void Error(string message, params object[] args);

        /// <summary>
        /// The fatal.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        void Fatal(string message, params object[] args);

        /// <summary>
        /// The info.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        void Info(string message, params object[] args);

        /// <summary>
        /// The warn.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        void Warn(string message, params object[] args);

        #endregion
    }
}