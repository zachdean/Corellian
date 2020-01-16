using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using Corellian;
using Corellian.Core.Extensions;
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
            PopModal = NavigationService.GetPopModalCommand();

            PopPage = NavigationService.GetPopPageCommand();

            PushPage = NavigationService.GetPushPageCommand<IRedViewModel>();

            PopToRoot = NavigationService.GetPopToRootPageCommand();

            PopModal.Subscribe(x => Debug.WriteLine("PagePushed"));
            PopModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            PopPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            PushPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            PopToRoot.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        }
    }
}
