using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using Corellian;
using ReactiveUI;

namespace CorellianSample.ViewModels
{
    public interface IHomeViewModel : IViewModel
    {
        ReactiveCommand<Unit, Unit> OpenModal { get; set; }
        ReactiveCommand<Unit, Unit> PushPage { get; set; }
    }

    public class HomeViewModel : ViewModelBase<IHomeViewModel>, IHomeViewModel
    {
        public ReactiveCommand<Unit, Unit> OpenModal { get; set; }

        public ReactiveCommand<Unit, Unit> PushPage { get; set; }

        public HomeViewModel(INavigationService viewStackService = null)
            : base(viewStackService)
        {
            OpenModal = ReactiveCommand
                .CreateFromObservable(() =>
                    this.NavigationService.PushModal<IFirstModalViewModel>(),
                    outputScheduler: RxApp.MainThreadScheduler);

            PushPage = ReactiveCommand
                .CreateFromObservable(() =>
                    this.NavigationService.PushPage<IRedViewModel>(),
                    outputScheduler: RxApp.MainThreadScheduler);

            PushPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            OpenModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        }
    }
}
