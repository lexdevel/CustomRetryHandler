using System;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace CustomRetryHandler
{
    /// <summary>
    /// The error detection strategy.
    /// </summary>
    public class TransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        #region ITransientErrorDetectionStrategy implementation

        /// <summary>
        /// Checks if the specified exception is transient.
        /// </summary>
        /// <param name="exception">The specified exception.</param>
        /// <returns>True if success, false otherwise.</returns>
        public bool IsTransient(Exception exception)
        {
            return exception is RetryException;
        }

        #endregion
    }
}
