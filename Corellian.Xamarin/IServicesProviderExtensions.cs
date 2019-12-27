using Microsoft.Extensions.DependencyInjection;
using System;

namespace Corellian.Xamarin
{
    public static class IServicesProviderExtensions
    {
        public static NavigationView GetNavigationView(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IView>() as NavigationView;
        }
    }
}
