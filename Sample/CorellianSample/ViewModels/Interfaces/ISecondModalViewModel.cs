using System.Reactive;
using ReactiveUI;
using Corellian;

namespace CorellianSample.ViewModels
{
    public interface ISecondModalViewModel : IViewModel
    {
        ReactiveCommand<Unit, Unit> PushPage { get; set; }

        ReactiveCommand<Unit, Unit> PopModal { get; set; }
    }
}
