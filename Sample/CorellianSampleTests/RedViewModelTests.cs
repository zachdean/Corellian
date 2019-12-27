using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using Corellian;
using CorellianSample;
using CorellianSample.ViewModels;
using Xunit;

namespace CorellianSampleTests
{
    public class RedViewModelTests
    {

        [Fact]
        public void PushPage_RedViewModel()
        {
            var viewNaviagtionService = Substitute.For<INavigationService>();
            var homeViewModel = new RedViewModel(viewNaviagtionService);
            homeViewModel.PushPage.Execute().Subscribe();
            viewNaviagtionService.Received().PushPage<RedViewModel>();

        }

        [Fact]
        public void PopModal_RedViewModel()
        {
            var viewNaviagtionService = Substitute.For<INavigationService>();
            var homeViewModel = new RedViewModel(viewNaviagtionService);
            homeViewModel.PopModal.Execute().Subscribe();
            viewNaviagtionService.Received().PopModal();
        }

        [Fact]
        public void PopPage_RedViewModel()
        {
            var viewNaviagtionService = Substitute.For<INavigationService>();
            var homeViewModel = new RedViewModel(viewNaviagtionService);
            homeViewModel.PopPage.Execute().Subscribe();
            viewNaviagtionService.Received().PopPage();
        }

        [Fact]
        public void PopToRoot_RedViewModel()
        {
            var viewNaviagtionService = Substitute.For<INavigationService>();
            var homeViewModel = new RedViewModel(viewNaviagtionService);
            homeViewModel.PopToRoot.Execute().Subscribe();
            viewNaviagtionService.Received().PopToRootPage();
        }

    }
}
