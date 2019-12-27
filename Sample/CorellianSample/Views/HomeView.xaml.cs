using ReactiveUI;
using ReactiveUI.XamForms;
using CorellianSample.ViewModels;
using System.Reactive.Disposables;

namespace CorellianSample.Views
{
    public partial class HomeView : ReactiveContentPage<IHomeViewModel>
    {
        public HomeView()
        {
            InitializeComponent();

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
