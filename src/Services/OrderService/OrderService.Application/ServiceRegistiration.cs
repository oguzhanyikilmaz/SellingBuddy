using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Features.Commands;
using System;
using System.Reflection;
using MediatR;

namespace OrderService.Application
{
    public static class ServiceRegistiration
    {
        public static IServiceCollection AddAplicationRegistiration(this IServiceCollection services,Type startup)
        {
            var assm = Assembly.GetExecutingAssembly();
            var assm2 = startup.GetTypeInfo().Assembly;

            services.AddAutoMapper(assm);
            services.AddMediatR(assm);

            return services;
        }
    }
}
