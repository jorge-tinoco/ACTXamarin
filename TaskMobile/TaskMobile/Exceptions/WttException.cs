using System;

namespace TaskMobile.Exceptions
{
    /// <summary>
    /// Custom WTT Exception.
    /// </summary>
    class WttException: Exception
    {
        public new string Message { get; set; }

        public Severity Severity { get; set; }

        /// <summary>
        /// Default value of severity = Low;
        /// </summary>
        public WttException()
        {
            Severity = Severity.Low;
        }

        /// <summary>
        /// New exception  with defined severity.
        /// </summary>
        /// <param name="severity">Exception severity.</param>
        public WttException(Severity severity)
        {
            Severity = severity;
        }

        /// <summary>
        /// Creates a new custom exception with low severity by default.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="severity">Exception severity.</param>
        public WttException(string message, Severity severity = Severity.Low)
        {
            Message = message;
            Severity = severity;
        }
    }
}
