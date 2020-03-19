using System.Reactive;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive.Linq;
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
                this.WhenAnyValue(x => x.ParameterString)
                .Select(x => !string.IsNullOrEmpty(x)),
                () => new NavigationParameter { { SecondModalViewModel.ParameterKey, ParameterString } });

            PopModal = NavigationService.GetPopModalCommand();

            OpenModal.Subscribe(x => Debug.WriteLine("PagePushed"));
            PopModal.Subscribe(x => Debug.WriteLine("PagePoped"));
            PopModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        }
    }
}
