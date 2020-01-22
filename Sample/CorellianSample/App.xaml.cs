using System;
using Corellian.Xamarin;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using CorellianSample.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CorellianSample
{
    public partial class App : Application
    {
        public IServiceCollection ServiceCollection { get; }
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public static IServiceProvider Provider { get; private set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public App()
        {
            InitializeComponent();

            RxApp.DefaultExceptionHandler = new CorellianDefaultExceptionHandler();

            var services = new ServiceCollection();

            services.AddCorellian()
                .AddViewModels()
                .AddViews();

            ServiceCollection = services;
            Provider = services.BuildServiceProvider();

            this.InitializeCorellian<IHomeViewModel>(Provider);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }

}
