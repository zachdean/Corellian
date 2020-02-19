using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System.Diagnostics;

namespace Corellian.Xamarin
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCorellian(this IServiceCollection services)
        {
            var watch = Stopwatch.StartNew();
            services.UseMicrosoftDependencyInjection();
            Debug.WriteLine("DI Setup Time: " + watch.ElapsedMilliseconds);

            watch = Stopwatch.StartNew();
            //register navigation view
            services.AddSingleton<IView>(sp => new NavigationView(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler, sp.GetRequiredService<IViewLocator>()));
            Debug.WriteLine("IView Setup Time: " + watch.ElapsedMilliseconds);

            watch = Stopwatch.StartNew();
            //register core items
            services.AddCorellianCore();

            Debug.WriteLine("Core Setup Time: " + watch.ElapsedMilliseconds);

            return services;
        }
    }
}
