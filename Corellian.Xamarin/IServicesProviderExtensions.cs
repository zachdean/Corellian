using Microsoft.Extensions.DependencyInjection;
using System;

namespace Corellian.Xamarin
{
    public static class IServicesProviderExtensions
    {
        public static NavigationServicePage GetNavigationView(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<IView>() as NavigationServicePage;
        }
    }
}
