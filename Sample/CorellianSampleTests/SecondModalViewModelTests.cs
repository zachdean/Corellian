using System;
using NSubstitute;
using Corellian;
using CorellianSample.ViewModels;
using Xunit;

namespace CorellianSampleTests
{
    public class SecondModalViewModelTests
    {

        [Fact]
        public void Push_RedViewModel_SecondModalViewModel()
        {
            var viewNaviagtionService = Substitute.For<INavigationService>();
            var secondViewModel = new SecondModalViewModel(viewNaviagtionService);
            secondViewModel.PushPage.Execute().Subscribe();
            viewNaviagtionService.Received().PushPage<RedViewModel>();
        }

        [Fact]
        public void PopModel_SecondModalViewModel()
        {
            var viewNaviagtionService = Substitute.For<INavigationService>();
            var secondViewModel = new SecondModalViewModel(viewNaviagtionService);
            secondViewModel.PopModal.Execute().Subscribe();
            viewNaviagtionService.Received().PopModal();
        }
    }
}
