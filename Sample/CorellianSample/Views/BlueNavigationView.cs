using System;
using System.Reactive.Concurrency;
using Corellian.Xamarin;
using ReactiveUI;
using Xamarin.Forms;

namespace CorellianSample.Views
{
    public class BlueNavigationView : NavigationView, IViewFor
    {
        public BlueNavigationView(IViewLocator viewLocator)
            : base(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler, viewLocator)
        {
            this.BarBackgroundColor = Color.Blue;
            this.BarTextColor = Color.White;
        }

        public object ViewModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
