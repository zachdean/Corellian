using System;
using System.Reactive;
using System.Reactive.Linq;
using NSubstitute;
using CorellianSample.ViewModels;
using Xunit;
using Splat;
using CorellianSample;
using Corellian;

namespace CorellianSampleTests
{
    public class HomeViewModelTests
    {
        [Fact]
        public void PushPage_RedViewModel()
        {
            var viewNaviagtionService = Substitute.For<INavigationService>();
            var homeViewModel = new HomeViewModel(viewNaviagtionService);
            homeViewModel.PushPage.Execute().Subscribe();
            viewNaviagtionService.Received().PushPage<IRedViewModel>();     
        }

        [Fact]
        public void OpenModal_RedViewModel()
        {
            var viewNaviagtionService = Substitute.For<INavigationService>();
            var homeViewModel = new HomeViewModel(viewNaviagtionService);
            homeViewModel.OpenModal.Execute().Subscribe();
            viewNaviagtionService.Received().PushModal<IFirstModalViewModel>();
        }
    }
}
