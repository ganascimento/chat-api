using System;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GADev.Chat.Api.Configurations
{
    public static class Log
    {
        public static void AddLog(this IServiceCollection services) {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddElmah(options => {
                options.Path = @"log";
            });

            services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
        }
    }
}