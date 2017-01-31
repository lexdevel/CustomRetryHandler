using System;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace CustomRetryHandler
{
    /// <summary>
    /// The error detection strategy.
    /// </summary>
    public class ErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        #region ITransientErrorDetectionStrategy implementation

        /// <summary>
        /// Checks if the specified exception is transient.
        /// </summary>
        /// <param name="exception">The specified exception.</param>
        /// <returns>True if success, false otherwise.</returns>
        public bool IsTransient(Exception exception)
        {
            if (exception is RetryException)
            {
                // Console.WriteLine($"Retrying attempt { (exception as RetryException).CurrentRetry }");
                return true;
            }

            return false;
        }

        #endregion
    }
}
