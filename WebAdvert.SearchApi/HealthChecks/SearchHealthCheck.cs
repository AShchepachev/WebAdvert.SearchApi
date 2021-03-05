using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAdvert.SearchApi.Services;

namespace WebAdvert.SearchApi.HealthChecks
{
    public class SearchHealthCheck : IHealthCheck
    {
        private readonly ISearchService _service;

        public SearchHealthCheck(ISearchService service)
        {
            _service = service;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var serviceIsOk = await _service.CheckHealthAsync();

            return new HealthCheckResult(serviceIsOk ? HealthStatus.Healthy : HealthStatus.Unhealthy);
        }
    }
}
