using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Forms;

namespace Corellian.Xamarin
{
    public static class ApplicationExtensions
    {

        public static void InitializeCorellian<TViewModel>(this Application app, IServiceProvider provider) where TViewModel : IViewModel
        {
            var navigationService = provider.GetRequiredService<INavigationService>();
            navigationService.PushPage<TViewModel>(resetStack: true).Subscribe();
            app.MainPage = provider.GetNavigationView();
        }
    }
}
