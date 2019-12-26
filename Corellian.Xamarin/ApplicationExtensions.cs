using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Corellian.Xamarin
{
    public static class ApplicationExtensions
    {

        public static void InitializeCorellian<TViewModel>(this Application app, IServiceProvider provider) where TViewModel : IViewModel
        {
            app.MainPage = provider.GetNavigationView();

            var navigationService = provider.GetRequiredService<INavigationService>();
            navigationService.PushPage<TViewModel>(true, false).Subscribe();
        }

    }
}
