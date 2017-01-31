using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

namespace CustomRetryHandler
{
    /// <summary>
    /// The retry handler.
    /// </summary>
    public class RetryHandler : HttpClientHandler
    {
        #region Public properties

        /// <summary>
        /// Gets the retry policy.
        /// </summary>
        public RetryPolicy Policy { get; }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="retryPolicy">The retry policy.</param>
        public RetryHandler(RetryPolicy retryPolicy)
        {
            this.Policy = retryPolicy;
        }

        #region Overrides

        /// <summary>
        /// Send the http request using the retry policy.
        /// </summary>
        /// <param name="request">The http request message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpResponseMessage = null as HttpResponseMessage;
            var currentRetry = 0;

            this.Policy.Retrying += (sender, args) => currentRetry = args.CurrentRetryCount;

            await this.Policy.ExecuteAsync(async () =>
            {
                try
                {
                    Console.WriteLine($"Current retry is { currentRetry }");

                    httpResponseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                    if (!httpResponseMessage.IsSuccessStatusCode)
                    {
                        throw new HttpRequestException($"HTTP call failed with status code { httpResponseMessage.StatusCode.ToString() }");
                    }

                    return httpResponseMessage;
                }
                catch (Exception exception)
                {
                    if (exception is TaskCanceledException || exception is HttpRequestException)
                    {
                        throw new RetryException(currentRetry);
                    }
                    throw exception;
                }
            } /*, cancellationToken (Do not pass the cancellation token here...) */).ConfigureAwait(false);

            return httpResponseMessage ?? throw new Exception("Fatal error.");
        }

        #endregion
    }
}
