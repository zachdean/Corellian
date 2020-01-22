using System.Reactive;
using ReactiveUI;
using System;
using System.Diagnostics;
using Corellian;
using Corellian.Core.Extensions;
using ReactiveUI.Fody.Helpers;

namespace CorellianSample.ViewModels
{
    public class FirstModalViewModel : ViewModelBase<IFirstModalViewModel>, IFirstModalViewModel
    {
        public ReactiveCommand<Unit, Unit> OpenModal { get; set; }

        public ReactiveCommand<Unit, Unit> PopModal { get; set; }

        [Reactive] public string? ParameterString { get; set; }

        public FirstModalViewModel(INavigationService viewStackService) : base(viewStackService)
        {
            OpenModal = NavigationService.GetPushModalCommand<ISecondModalViewModel>(
                () => new NavigationParameter { { SecondModalViewModel.ParameterKey, ParameterString } });

            PopModal = NavigationService.GetPopModalCommand();

            OpenModal.Subscribe(x => Debug.WriteLine("PagePushed"));
            PopModal.Subscribe(x => Debug.WriteLine("PagePoped"));
            PopModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        }
    }
}
