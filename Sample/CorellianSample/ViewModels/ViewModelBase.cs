using System;
using Corellian;
using ReactiveUI;

namespace CorellianSample.ViewModels
{
	public abstract class ViewModelBase<T> : ReactiveObject, ILocatable where T : IViewModel
    {      
        protected readonly INavigationService NavigationService;

		protected ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public Type ViewModelInterface => typeof(T);

        public string Id => GetType().Name;
    }
}
