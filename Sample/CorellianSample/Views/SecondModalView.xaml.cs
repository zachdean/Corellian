using ReactiveUI;
using ReactiveUI.XamForms;
using CorellianSample.ViewModels;

namespace CorellianSample.Views
{
	public partial class SecondModalView : ReactiveContentPage<ISecondModalViewModel>
    {
		public SecondModalView()
        {
            InitializeComponent();
			this.BindCommand(ViewModel, x => x.PushPage, x => x.PushPage);
            this.BindCommand(ViewModel, x => x.PopModal, x => x.PopModal);

            Interactions
                .ErrorMessage
                .RegisterHandler(async x =>
                {
                    await DisplayAlert("Error", x.Input.Message, "Done");
                    x.SetOutput(true);
                });
        }
    }
}
