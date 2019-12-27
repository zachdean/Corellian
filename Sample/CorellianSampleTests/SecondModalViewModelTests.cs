using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using Corellian;
using CorellianSample.ViewModels;
using Xunit;

namespace CorellianSampleTests
{
    public class SecondModalViewModelTests
    {
        //SecondModalViewModel

        [Fact]
        public void Push_RedViewModel_SecondModalViewModel()
        {
            var viewNaviagtionService = Substitute.For<IViewStackService>();
            var secondViewModel = new SecondModalViewModel(viewNaviagtionService);
            secondViewModel.PushPage.Execute().Subscribe();
            viewNaviagtionService.Received().PushPage(Arg.Any<RedViewModel>());
        }

        [Fact]
        public void PopModel_SecondModalViewModel()
        {
            var viewNaviagtionService = Substitute.For<IViewStackService>();
            var secondViewModel = new SecondModalViewModel(viewNaviagtionService);
            secondViewModel.PopModal.Execute().Subscribe();
            viewNaviagtionService.Received().PopModal();
        }
    }
}
