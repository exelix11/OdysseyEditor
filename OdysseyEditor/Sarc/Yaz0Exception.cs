using System;
using System.Globalization;

namespace Syroot.NintenTools.Yaz0
{
    /// <summary>
    /// Represents an exception caused by invalid Yaz0 data.
    /// </summary>
    public class Yaz0Exception : Exception
    {
        // ---- CONSTRUCTORS -------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Yaz0Exception"/> class.
        /// </summary>
        public Yaz0Exception()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Yaz0Exception"/> class with the given message.
        /// </summary>
        /// <param name="message">The message provided with the exception.</param>
        public Yaz0Exception(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Yaz0Exception"/> class with the given message and inner
        /// exception.
        /// </summary>
        /// <param name="message">The message provided with the exception.</param>
        /// <param name="innerException">The inner exception.</param>
        public Yaz0Exception(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Yaz0Exception"/> class with the given message.
        /// </summary>
        /// <param name="format">A composite format string representing the message.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public Yaz0Exception(string format, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, format, args))
        {
        }
    }
}