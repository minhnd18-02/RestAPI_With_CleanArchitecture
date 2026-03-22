using Microsoft.Extensions.DependencyInjection;
using RestAPIService.Domain.Interfaces;
using RestAPIService.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<IOrderItemRepo, OrderItemRepo>();
            services.AddScoped<IOrderRepo, OrderRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<IRoleRepo, RoleRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
