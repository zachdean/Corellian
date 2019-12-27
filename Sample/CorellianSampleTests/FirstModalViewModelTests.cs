using System;
using NSubstitute;
using CorellianSample.ViewModels;
using Xunit;
using Corellian;

namespace CorellianSampleTests
{
    public class FirstModalViewModelTests
    {
        [Fact]
        public void PushModel_SecondModalViewModel()
        {
            var viewNaviagtionService = Substitute.For<INavigationService>();
            var firstViewModel = new FirstModalViewModel(viewNaviagtionService);
            firstViewModel.OpenModal.Execute().Subscribe();
            viewNaviagtionService.Received().PushModal<SecondModalViewModel>();
        }

        [Fact]
        public void PopModel_FirstModalViewModel()
        {
            var viewNaviagtionService = Substitute.For<INavigationService>();
            var firstViewModel = new FirstModalViewModel(viewNaviagtionService);
            firstViewModel.PopModal.Execute().Subscribe();
            viewNaviagtionService.Received().PopModal();
        }
    }
}
