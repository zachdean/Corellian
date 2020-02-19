using Corellian.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace Corellian
{
    public static class IServiceCollectionExtensions
    {

        public static IServiceCollection AddView<TViewModel, TView>(this IServiceCollection services) where TViewModel : class where TView : class, IViewFor<TViewModel>
        {
            services.AddTransient<IViewFor<TViewModel>, TView>();
            return services;
        }

        internal static IServiceCollection UseMicrosoftDependencyInjection(this IServiceCollection services)
        {
            //sets MS DI as the splat resolver
            Task.Run(() =>
            {
                services.UseMicrosoftDependencyResolver();
                var resolver = Locator.CurrentMutable;
                resolver.InitializeSplat();
            });

            //Add the reactiveui dependancies to our container
            services.AddReactiveUI();
            services.AddTransient<ILogger, NullLogger>();
            services.AddSingleton<IFullLogger, WrappingFullLogger>();

            return services;
        }

        internal static IServiceCollection AddCorellianCore(this IServiceCollection services)
        {
            if (!services.Any(x => x.ServiceType == typeof(IView)))
            {
                throw new InvalidOperationException("IView must be registered to use Corellian!");
            }

            if (!services.Any(x => x.ServiceType == typeof(IFullLogger)))
            {
                throw new InvalidOperationException("IFullLogger must be registered to use Corellian!");
            }

            services.AddSingleton<IViewLocator, Core.Services.ViewLocator>();
            services.AddSingleton<INavigationService, NavigationService>();
            return services;
        }

        internal static IServiceCollection AddReactiveUI(this IServiceCollection services)
        {
            services.AddSingleton<ICreatesObservableForProperty, INPCObservableForProperty>();
            services.AddSingleton<ICreatesObservableForProperty, IROObservableForProperty>();
            services.AddSingleton<ICreatesObservableForProperty, POCOObservableForProperty>();
            services.AddSingleton<IBindingTypeConverter, EqualityTypeConverter>();
            services.AddSingleton<IBindingTypeConverter, StringConverter>();
            services.AddSingleton<IActivationForViewFetcher, CanActivateViewFetcher>();
            services.AddSingleton<ICreatesCommandBinding, CreatesCommandBindingViaEvent>();
            services.AddSingleton<ICreatesCommandBinding, CreatesCommandBindingViaCommandParameter>();

            RxApp.TaskpoolScheduler = TaskPoolScheduler.Default;
            RxApp.MainThreadScheduler = DefaultScheduler.Instance;

            return services;
        }
    }
}
