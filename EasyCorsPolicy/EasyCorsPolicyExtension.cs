using System;
using EasyCorsPolicy.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyCorsPolicy
{
    public static class EasyCorsPolicyExtension
    {
        /// <summary>
        /// Adds cross-origin resource sharing services to the specified services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void AddEasyCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPolicyService>(opt => new PolicyService(configuration));

            IPolicyService policyService = services.BuildServiceProvider()
                                                   .GetRequiredService<IPolicyService>();

            services.AddCors(options => policyService.AddPolicies(options));
        }

        /// <summary>
        /// Adds a CORS middleware to your web application pipeline to allow cross domain requests.
        /// </summary>
        /// <param name="applicationBuilder">The IApplicationBuilder passed to your Configure method</param>
        public static void UseEasyCors(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseCors();
        }
    }
}