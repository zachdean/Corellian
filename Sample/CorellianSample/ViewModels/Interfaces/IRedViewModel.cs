using System;
using System.Reactive;
using Corellian;
using ReactiveUI;

namespace CorellianSample.ViewModels
{
    public interface IRedViewModel : IViewModel
    {
        ReactiveCommand<Unit, Unit> PopModal { get; set; }
        ReactiveCommand<Unit, Unit> PopPage { get; set; }
        ReactiveCommand<Unit, Unit> PopToRoot { get; set; }
        ReactiveCommand<Unit, Unit> PushPage { get; set; }
    }
}
