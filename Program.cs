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
        /// The http client.
        /// </summary>
        internal static readonly HttpClient httpClient = new HttpClient(new RetryHandler(new RetryPolicy(new ErrorDetectionStrategy(), RetryCount)));

        /// <summary>
        /// The request url for making http calls.
        /// </summary>
        internal static readonly string RequestUrl = "http://localhost";

        #endregion

        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        internal static void Main(string[] args)
        {
            httpClient.Timeout = TimeSpan.FromSeconds(4);

            try
            {
                using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, RequestUrl))
                using (var response = httpClient.SendAsync(httpRequestMessage).GetAwaiter().GetResult())
                {
                    Console.WriteLine("OK");
                }
            }
            catch (Exception exception)
            {
                if (exception is RetryException)
                {
                    Console.WriteLine($"Error: maximum retry count reached ({ (exception as RetryException).CurrentRetry } of { RetryCount })");
                }
                else
                {
                    Console.WriteLine($"Error: { exception.Message }");
                }
            }
        }
    }
}
