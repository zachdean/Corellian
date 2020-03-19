using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace Corellian.Core.Extensions
{
    public static class INavigationServiceExtensions
    {
        public static ReactiveCommand<Unit, Unit> GetPushModalCommand<T>(
            this INavigationService service, 
            Func<INavigationParameter>? parameter = null,
            bool withNavigationPage = true) where T : IViewModel
        {
            return ReactiveCommand.CreateFromObservable(() =>
                    service.PushModal<T>(parameter?.Invoke(), withNavigationPage),
                service.CanNavigate.ObserveOn(RxApp.MainThreadScheduler),
                outputScheduler: RxApp.MainThreadScheduler);
        }

        public static ReactiveCommand<Unit, Unit> GetPushModalCommand<T>(
            this INavigationService service,
            IObservable<bool> canExecute,
            Func<INavigationParameter>? parameter = null, 
            bool withNavigationPage = true) where T : IViewModel
        {
            return ReactiveCommand.CreateFromObservable(() =>
                    service.PushModal<T>(parameter?.Invoke(), withNavigationPage),
                //service.CanNavigate
                //    .ObserveOn(RxApp.MainThreadScheduler)
                //    .CombineLatest(canExecute.ObserveOn(RxApp.MainThreadScheduler), (navigate, execute) => navigate && execute),
                outputScheduler: RxApp.MainThreadScheduler);
        }

        public static ReactiveCommand<Unit, Unit> GetPushPageCommand<T>(
            this INavigationService service,
            Func<INavigationParameter>? parameter = null, 
            bool resetStack = false, 
            bool animate = true) where T : IViewModel
        {
            return ReactiveCommand.CreateFromObservable(() =>
                    service.PushPage<T>(parameter?.Invoke(), resetStack, animate),
                service.CanNavigate.ObserveOn(RxApp.MainThreadScheduler),
                outputScheduler: RxApp.MainThreadScheduler);
        }

        public static ReactiveCommand<Unit, Unit> GetPushPageCommand<T>(this INavigationService service,
            IObservable<bool> canExecute,
            Func<INavigationParameter>? parameter = null,
            bool resetStack = false, 
            bool animate = true) where T : IViewModel
        {
            return ReactiveCommand.CreateFromObservable(() =>
                    service.PushPage<T>(parameter?.Invoke(), resetStack, animate),
                service.CanNavigate
                    .CombineLatest(canExecute, (navigate, execute) => navigate && execute)
                    .ObserveOn(RxApp.MainThreadScheduler),
                outputScheduler: RxApp.MainThreadScheduler);
        }

        public static ReactiveCommand<Unit, Unit> GetPopModalCommand(this INavigationService service, bool animate = true) =>
            ReactiveCommand.CreateFromObservable(() =>
                            service.PopModal(animate),
                            service.CanNavigate.ObserveOn(RxApp.MainThreadScheduler),
                            outputScheduler: RxApp.MainThreadScheduler);

        public static ReactiveCommand<Unit, Unit> GetPopPageCommand(this INavigationService service, bool animate = true) =>
            ReactiveCommand.CreateFromObservable(() =>
                            service.PopPage(animate),
                            service.CanNavigate.ObserveOn(RxApp.MainThreadScheduler),
                            outputScheduler: RxApp.MainThreadScheduler);

        public static ReactiveCommand<Unit, Unit> GetPopToRootPageCommand(this INavigationService service, bool animate = true) =>
            ReactiveCommand.CreateFromObservable(() =>
                            service.PopToRootPage(animate),
                            service.CanNavigate.ObserveOn(RxApp.MainThreadScheduler),
                            outputScheduler: RxApp.MainThreadScheduler);
    }
}
