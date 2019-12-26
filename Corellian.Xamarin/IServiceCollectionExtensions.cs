using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Corellian.Extensions;
using ReactiveUI;

namespace Corellian.Xamarin
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCorellian(this IServiceCollection services)
        {
            services.UseMicrosoftDependencyInjection();

            //register navigation view
            services.AddSingleton<IView>(sp => new NavigationView(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler, sp.GetRequiredService<IViewLocator>()));

            //register core items
            services.AddCorellianCore();            

            return services;
        }
    }
}
