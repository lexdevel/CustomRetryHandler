using System;

namespace CustomRetryHandler
{
    /// <summary>
    /// The retry exception.
    /// </summary>
    public class RetryException : Exception
    {
        #region Public properties

        /// <summary>
        /// Gets the current retry count.
        /// </summary>
        public int CurrentRetry { get; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="currentRetry">The current retry count.</param>
        public RetryException(int currentRetry)
        {
            CurrentRetry = currentRetry;
        }
    }
}
