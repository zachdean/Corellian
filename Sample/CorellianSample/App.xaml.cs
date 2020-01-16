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
        public static IServiceProvider Provider { get; private set; }

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
