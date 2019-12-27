using System.Reactive;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using Corellian;

namespace CorellianSample.ViewModels
{

    public interface ISecondModalViewModel : IViewModel
    {
        ReactiveCommand<Unit, Unit> PushPage
        {
            get;
            set;
        }

        ReactiveCommand<Unit, Unit> PopModal
        {
            get;
            set;
        }
    }

    public class SecondModalViewModel : ViewModelBase<ISecondModalViewModel>, ISecondModalViewModel
    {
		public ReactiveCommand<Unit, Unit> PushPage
        {
            get;
            set;
        }

		public ReactiveCommand<Unit, Unit> PopModal
        {
            get;
            set;
        }

		public SecondModalViewModel(INavigationService viewStackService) : base(viewStackService)
		{
			PushPage = ReactiveCommand
				.CreateFromObservable(() =>
	                this.NavigationService.PushPage<IRedViewModel>(),
                    outputScheduler: RxApp.MainThreadScheduler);

			PopModal = ReactiveCommand
				.CreateFromObservable(() =>
	                this.NavigationService.PopModal(),
                    outputScheduler: RxApp.MainThreadScheduler);

            PushPage.Subscribe(x => Debug.WriteLine("PagePushed"));
			PopModal.Subscribe(x => Debug.WriteLine("PagePoped"));

		    PushPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
		    PopModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        }
	}
}
