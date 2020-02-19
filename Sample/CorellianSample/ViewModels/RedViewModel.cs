using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using Corellian;
using ReactiveUI;

namespace CorellianSample.ViewModels
{
    public class RedViewModel : ViewModelBase<IRedViewModel>, IRedViewModel
    {
        public ReactiveCommand<Unit, Unit> PopModal { get; set; }

        public ReactiveCommand<Unit, Unit> PushPage { get; set; }

        public ReactiveCommand<Unit, Unit> PopPage { get; set; }

        public ReactiveCommand<Unit, Unit> PopToRoot { get; set; }

        public RedViewModel(INavigationService viewStackService) : base(viewStackService)
        {
            PopModal = ReactiveCommand
                .CreateFromObservable(() =>
                    this.NavigationService.PopModal(),
                    outputScheduler: RxApp.MainThreadScheduler);

            PopPage = ReactiveCommand
                .CreateFromObservable(() =>
                    this.NavigationService.PopPage(),
                    outputScheduler: RxApp.MainThreadScheduler);

            PushPage = ReactiveCommand
                .CreateFromObservable(() =>
                    this.NavigationService.PushPage<IRedViewModel>(),
                    outputScheduler: RxApp.MainThreadScheduler);

            PopToRoot = ReactiveCommand
                .CreateFromObservable(() =>
                    this.NavigationService.PopToRootPage(),
                    outputScheduler: RxApp.MainThreadScheduler);

            PopModal.Subscribe(x => Debug.WriteLine("PagePushed"));
            PopModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            PopPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            PushPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            PopToRoot.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        }
    }
}
