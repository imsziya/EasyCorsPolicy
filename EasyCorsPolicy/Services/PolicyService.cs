using EasyCorsPolicy.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace EasyCorsPolicy.Services
{
    internal class PolicyService : IPolicyService
    {
        private readonly IConfiguration _configuration;
        private static string ConfigSectionName => "EasyCors";

        public PolicyService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void AddPolicies(CorsOptions options)
        {
            Dictionary<string, ConfigurationData> dataDict = GetConfiguration();
            foreach (KeyValuePair<string, ConfigurationData> data in dataDict)
            {
                if (data.Value.IsDefault)
                {
                    options.AddDefaultPolicy(policyBuilder =>
                    {
                        AddPolicies(policyBuilder, data.Value);
                    });
                }
                else
                {
                    options.AddPolicy(name: data.Key, policyBuilder =>
                    {
                        AddPolicies(policyBuilder, data.Value);
                    });
                }
            }
        }

        private void AddPolicies(CorsPolicyBuilder policyBuilder, ConfigurationData data)
        {
            if (!string.IsNullOrEmpty(data.AllowedOrigins))
            {
                if (data.AllowedOrigins == "*")
                {
                    policyBuilder.AllowAnyOrigin();
                }
                else
                {
                    var allowedOrigins = data.AllowedOrigins.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    policyBuilder.WithOrigins(allowedOrigins);
                }
            }
            if (!string.IsNullOrEmpty(data.AllowedMethods))
            {
                if (data.AllowedMethods == "*")
                {
                    policyBuilder.AllowAnyMethod();
                }
                else
                {
                    var allowedMethods = data.AllowedMethods.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    policyBuilder.WithMethods(allowedMethods);
                }
            }
            if (!string.IsNullOrEmpty(data.AllowedHeaders))
            {
                if (data.AllowedHeaders == "*")
                {
                    policyBuilder.AllowAnyHeader();
                }
                else
                {
                    var allowedHeaders = data.AllowedHeaders.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    policyBuilder.WithHeaders(allowedHeaders);
                }
            }
            if (!string.IsNullOrEmpty(data.AllowedExposedHeaders))
            {
                var allowedExposedHeaders = data.AllowedExposedHeaders.Split(",", StringSplitOptions.RemoveEmptyEntries);
                if (allowedExposedHeaders.Length != 0)
                {
                    policyBuilder.WithExposedHeaders(allowedExposedHeaders);
                }
            }
            if (data.IsAllowedCredentials && data.AllowedOrigins != "*")
            {
                policyBuilder.AllowCredentials();
            }
            else policyBuilder.DisallowCredentials();
        }

        private Dictionary<string, ConfigurationData> GetConfiguration()
        {
            string policyData = _configuration[ConfigSectionName] ?? throw new ArgumentException("Please Add EasyCors policies in your project configuration!");
            return JsonSerializer.Deserialize<Dictionary<string, ConfigurationData>>(policyData);
        }
    }
}