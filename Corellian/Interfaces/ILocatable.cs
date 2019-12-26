using System;

namespace Corellian
{
    public interface ILocatable : IViewModel
    {
        Type ViewModelInterface { get; }
    }
}