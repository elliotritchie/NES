// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logger.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The logger.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     The logger.
    /// </summary>
    public class Logger : ILogger
    {
        #region Fields

        private readonly Action<string, object[]> _debug;

        private readonly Action<string, object[]> _error;

        private readonly Action<string, object[]> _fatal;

        private readonly Action<string, object[]> _info;

        private readonly Action<string, object[]> _warn;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="debug">
        /// The debug.
        /// </param>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="warn">
        /// The warn.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <param name="fatal">
        /// The fatal.
        /// </param>
        public Logger(
            Action<string, object[]> debug, 
            Action<string, object[]> info, 
            Action<string, object[]> warn, 
            Action<string, object[]> error, 
            Action<string, object[]> fatal)
        {
            this._debug = debug;
            this._info = info;
            this._warn = warn;
            this._error = error;
            this._fatal = fatal;
        }

        #endregion

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
        public void Debug(string message, params object[] args)
        {
            this._debug(message, args);
        }

        /// <summary>
        /// The error.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void Error(string message, params object[] args)
        {
            this._error(message, args);
        }

        /// <summary>
        /// The fatal.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void Fatal(string message, params object[] args)
        {
            this._fatal(message, args);
        }

        /// <summary>
        /// The info.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void Info(string message, params object[] args)
        {
            this._info(message, args);
        }

        /// <summary>
        /// The warn.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        public void Warn(string message, params object[] args)
        {
            this._warn(message, args);
        }

        #endregion
    }
}