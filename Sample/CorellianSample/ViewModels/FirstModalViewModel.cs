using System.Reactive;
using ReactiveUI;
using System;
using System.Diagnostics;
using Corellian;

namespace CorellianSample.ViewModels
{
    public class FirstModalViewModel : ViewModelBase<IFirstModalViewModel>, IFirstModalViewModel
    {
        public ReactiveCommand<Unit, Unit> OpenModal { get; set; }

        public ReactiveCommand<Unit, Unit> PopModal { get; set; }

        public FirstModalViewModel(INavigationService viewStackService) : base(viewStackService)
        {
            OpenModal = ReactiveCommand
                        .CreateFromObservable(() =>
                            this.NavigationService.PushModal<ISecondModalViewModel>(),
                            NavigationService.CanNavigate,
                            outputScheduler: RxApp.MainThreadScheduler);

            PopModal = ReactiveCommand
                        .CreateFromObservable(() =>
                            this.NavigationService.PopModal(),
                            outputScheduler: RxApp.MainThreadScheduler);

            OpenModal.Subscribe(x => Debug.WriteLine("PagePushed"));
            PopModal.Subscribe(x => Debug.WriteLine("PagePoped"));
            PopModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        }
    }
}
