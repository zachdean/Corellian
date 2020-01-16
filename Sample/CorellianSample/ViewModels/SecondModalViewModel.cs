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

    public class SecondModalViewModel : ViewModelBase<ISecondModalViewModel>, ISecondModalViewModel, INavigatable
    {
        public const string ParameterKey = "Example";

        public ReactiveCommand<Unit, Unit> PushPage { get; set; }

        public ReactiveCommand<Unit, Unit> PopModal { get; set; }

        [Reactive] public string PassedParameter { get; set; }

        public SecondModalViewModel(INavigationService viewStackService) : base(viewStackService)
        {
            PushPage = NavigationService.GetPushPageCommand<IRedViewModel>();

            PopModal = NavigationService.GetPopModalCommand();

            PushPage.Subscribe(x => Debug.WriteLine("PagePushed"));
            PopModal.Subscribe(x => Debug.WriteLine("PagePoped"));

            PushPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            PopModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        }

        public void WhenNavigatingTo(INavigationParameter parameter)
        {
            PassedParameter = parameter.GetRequiredParameter<string>(ParameterKey);
        }
    }
}
