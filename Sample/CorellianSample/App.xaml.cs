using System;
using Corellian.Xamarin;
using Corellian.Extensions;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using CorellianSample.ViewModels;
using CorellianSample.Views;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
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

            services.AddCorellian();

            services.AddViews();

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

    public static class ApplicationExtensions
    {
        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            services.AddView<IHomeViewModel, HomeView>();
            services.AddView<IFirstModalViewModel, FirstModalView>();
            services.AddView<ISecondModalViewModel, SecondModalView>();
            services.AddView<IRedViewModel, RedView>();

            services.AddTransient<IFirstModalViewModel, FirstModalViewModel>();
            services.AddTransient<IHomeViewModel, HomeViewModel>();
            services.AddTransient<ISecondModalViewModel, SecondModalViewModel>();
            services.AddTransient<IRedViewModel, RedViewModel>();
            return services;
        }
    }


}
