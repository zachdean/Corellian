using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Corellian;
using Corellian.Core.Extensions;
using ReactiveUI;

namespace CorellianSample.ViewModels
{

    public class HomeViewModel : ViewModelBase<IHomeViewModel>, IHomeViewModel
    {
        public ReactiveCommand<Unit, Unit> OpenModal { get; set; }

        public ReactiveCommand<Unit, Unit> PushPage { get; set; }

        public HomeViewModel(INavigationService viewStackService)
            : base(viewStackService)
        {
            OpenModal = NavigationService.GetPushModalCommand<IFirstModalViewModel>();

            PushPage = NavigationService.GetPushPageCommand<IRedViewModel>();

            PushPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            OpenModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        }
    }
}
