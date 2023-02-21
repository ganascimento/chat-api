using System;
using GADev.Chat.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace GADev.Chat.Api.Configurations
{
    public static class DependencyInjectionSetup
    {
        public static void AddDependencyInjectionSetup(this IServiceCollection services){
            if (services == null) throw new ArgumentNullException(nameof(services));

            NativeInjector.RegisterServices(services);
        }
    }
}