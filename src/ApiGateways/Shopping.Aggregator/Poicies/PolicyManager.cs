﻿using System;
using Polly;
using Polly.Extensions.Http;

namespace Shopping.Aggregator.Poicies
{
	public static class PolicyManager
	{
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ILogger logger)
        {
            // In this case will wait for
            // 2 ^ 1 = 2 seconds then
            // 2 ^ 2 = 4 seconds then
            // 2 ^ 3 = 8 seconds then
            // 2 ^ 4 = 16 seconds then
            // 2 ^ 5 = 32 seconds
            return HttpPolicyExtensions
                     .HandleTransientHttpError()
                     .WaitAndRetryAsync(
                         retryCount: 5,
                         sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                         onRetry: (exception, retryCount, context) =>
                         {
                            logger.LogError($"Retry { retryCount} of { context.PolicyKey} at { context.OperationKey}, due to: { exception}.");
                         }
                     );
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .CircuitBreakerAsync(
                        handledEventsAllowedBeforeBreaking: 5,
                        durationOfBreak: TimeSpan.FromSeconds(30)
                    );
        }
    }
}

