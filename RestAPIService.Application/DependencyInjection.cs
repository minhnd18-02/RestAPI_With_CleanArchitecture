using Microsoft.Extensions.DependencyInjection;
using RestAPIService.Application.IServices;
using RestAPIService.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection Services(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
