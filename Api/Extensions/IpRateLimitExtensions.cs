using AspNetCoreRateLimit;

namespace Api.Extensions
{
    public static class IpRateLimitExtensions
    {
        public static IServiceCollection AddIpRateLimit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            services.Configure<IpRateLimitOptions>(options =>
            {
                //EnableEndpointRateLimiting - define rules for specific endpoint
                //StackBlockedRequests - if true requests that exceed the limit are queued and processed as soon as the counter allows
                options.EnableEndpointRateLimiting = false; 
                options.StackBlockedRequests = false; 

            });
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimit"));
            // Rate Limiting Dependencies:
            // IIpPolicyStore - Stores rate limit policies (rules, limits, whitelists)
            // IRateLimitCounterStore - Tracks request counts per IP/endpoint  
            // IRateLimitConfiguration - Provides additional configuration (client ID resolution, custom headers)
            // IProcessingStrategy - Handles concurrency and prevents race conditions when updating counters
            // AddInMemoryRateLimiting - Registers in-memory implementations for all rate limiting services
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddInMemoryRateLimiting();
            return services;
        }
    }
}
