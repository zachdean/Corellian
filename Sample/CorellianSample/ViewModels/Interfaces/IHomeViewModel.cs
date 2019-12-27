using System.Reactive;
using Corellian;
using ReactiveUI;

namespace CorellianSample.ViewModels
{
    public interface IHomeViewModel : IViewModel
    {
        ReactiveCommand<Unit, Unit> OpenModal { get; set; }
        ReactiveCommand<Unit, Unit> PushPage { get; set; }
    }
}
