using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corellian.Core.Services
{
    internal class ViewLocator : IViewLocator
    {
        private readonly IServiceProvider serviceProvider;
        public ViewLocator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IViewFor ResolveView<T>(T viewModel, string contract = null) where T : class
        {
            //get viewfor interface
            Type iViewFor = typeof(IViewFor<>).MakeGenericType(viewModel is ILocatable intefaceLocation ? intefaceLocation.ViewModelInterface : viewModel.GetType());

            //get registered view
            return serviceProvider.GetRequiredService(iViewFor) as IViewFor;

        }
    }
}
