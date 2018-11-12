using System;
using System.Net.Http;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace CustomRetryHandler
{
    /// <summary>
    /// Application entry point class.
    /// </summary>
    internal static class Program
    {
        #region Internal static fields

        /// <summary>
        /// The retry count.
        /// </summary>
        internal static readonly int RetryCount = 4;

        /// <summary>
        /// The retry handler.
        /// </summary>
        internal static RetryHandler RetryHandler = new RetryHandler(new RetryPolicy(new TransientErrorDetectionStrategy(), RetryCount));

        /// <summary>
        /// The http client.
        /// </summary>
        internal static readonly HttpClient HttpClient = new HttpClient(RetryHandler);

        /// <summary>
        /// The request url for making http calls.
        /// </summary>
        internal static readonly string RequestUrl = "http://localhost";

        #endregion

        /// <summary>
        /// Application entry point.
        /// </summary>
        internal static void Main()
        {
            HttpClient.Timeout = TimeSpan.FromSeconds(4);

            try
            {
                using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, RequestUrl))
                using (HttpClient.SendAsync(httpRequestMessage).GetAwaiter().GetResult())
                {
                    Console.WriteLine("OK");
                }
            }
            catch (Exception exception)
            {
                var retryException = exception as RetryException;
                var errorMessage = retryException != null
                    ? $"Error: maximum retry count reached ({retryException.CurrentRetry} of {RetryCount})"
                    : $"Error: {exception.Message}";

                Console.WriteLine($"Error: {errorMessage}");
            }
        }
    }
}
