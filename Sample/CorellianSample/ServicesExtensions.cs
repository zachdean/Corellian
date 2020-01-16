using CorellianSample.ViewModels;
using CorellianSample.Views;
using Corellian;
using Microsoft.Extensions.DependencyInjection;

namespace CorellianSample
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            services.AddView<IHomeViewModel, HomeView>();
            services.AddView<IFirstModalViewModel, FirstModalView>();
            services.AddView<ISecondModalViewModel, SecondModalView>();
            services.AddView<IRedViewModel, RedView>();
            return services;
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddTransient<IFirstModalViewModel, FirstModalViewModel>();
            services.AddTransient<IHomeViewModel, HomeViewModel>();
            services.AddTransient<ISecondModalViewModel, SecondModalViewModel>();
            services.AddTransient<IRedViewModel, RedViewModel>();
            return services;
        }

    }
}
