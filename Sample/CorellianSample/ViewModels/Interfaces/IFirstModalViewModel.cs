using System.Reactive;
using ReactiveUI;
using Corellian;

namespace CorellianSample.ViewModels
{
    public interface IFirstModalViewModel : IViewModel
    {
        ReactiveCommand<Unit, Unit> OpenModal { get; set; }
        ReactiveCommand<Unit, Unit> PopModal { get; set; }
    }
}
