using Microsoft.AspNetCore.Cors.Infrastructure;

namespace EasyCorsPolicy.Services
{
    internal interface IPolicyService
    {
        public void AddPolicies(CorsOptions options);
    }
}