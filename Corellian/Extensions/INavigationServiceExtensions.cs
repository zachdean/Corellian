using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace Corellian.Core.Extensions
{
    public static class INavigationServiceExtensions
    {
        public static ReactiveCommand<Unit, Unit> GetPushModalCommand<T>(this INavigationService service, Func<INavigationParameter>? parameter = null, bool withNavigationPage = true) where T : IViewModel =>
            ReactiveCommand.CreateFromObservable(() =>
#pragma warning disable CS8604 // Possible null reference argument.
                            service.PushModal<T>(parameter?.Invoke(), withNavigationPage),
#pragma warning restore CS8604 // Possible null reference argument.
                            service.CanNavigate,
                            outputScheduler: RxApp.MainThreadScheduler);

        public static ReactiveCommand<Unit, Unit> GetPushPageCommand<T>(this INavigationService service, Func<INavigationParameter>? parameter = null, bool resetStack = false, bool animate = true) where T : IViewModel =>
            ReactiveCommand.CreateFromObservable(() =>
#pragma warning disable CS8604 // Possible null reference argument.
                            service.PushPage<T>(parameter?.Invoke(), resetStack, animate),
#pragma warning restore CS8604 // Possible null reference argument.
                            service.CanNavigate,
                            outputScheduler: RxApp.MainThreadScheduler);

        public static ReactiveCommand<Unit, Unit> GetPopModalCommand(this INavigationService service, bool animate = true) =>
            ReactiveCommand.CreateFromObservable(() =>
                            service.PopModal(animate),
                            service.CanNavigate,
                            outputScheduler: RxApp.MainThreadScheduler);

        public static ReactiveCommand<Unit, Unit> GetPopPageCommand(this INavigationService service, bool animate = true) =>
            ReactiveCommand.CreateFromObservable(() =>
                            service.PopPage(animate),
                            service.CanNavigate,
                            outputScheduler: RxApp.MainThreadScheduler);

        public static ReactiveCommand<Unit, Unit> GetPopToRootPageCommand(this INavigationService service, bool animate = true) =>
            ReactiveCommand.CreateFromObservable(() =>
                            service.PopToRootPage(animate),
                            service.CanNavigate,
                            outputScheduler: RxApp.MainThreadScheduler);
    }
}
