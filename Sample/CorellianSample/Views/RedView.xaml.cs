using ReactiveUI;
using ReactiveUI.XamForms;
using CorellianSample.ViewModels;

namespace CorellianSample.Views
{
	public partial class RedView : ReactiveContentPage<IRedViewModel>, IViewFor<RedViewModel>
    {
        public RedView()
        {
            InitializeComponent();
			this.BindCommand(ViewModel, x => x.PopModal, x => x.PopModal);
            this.BindCommand(ViewModel, x => x.PushPage, x => x.PushPage);
            this.BindCommand(ViewModel, x => x.PopPage, x => x.PopPage);
            this.BindCommand(ViewModel, x => x.PopToRoot, x => x.PopToRoot);

            Interactions
                .ErrorMessage
                .RegisterHandler(async x =>
                {
                    await DisplayAlert("Error", x.Input.Message, "Done");
                    x.SetOutput(true);
                });
        }

        RedViewModel IViewFor<RedViewModel>.ViewModel { get => ViewModel as RedViewModel; set => ViewModel = value; }
    }
}
