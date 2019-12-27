using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using CorellianSample.ViewModels;
using Xunit;

namespace CorellianSampleTests
{
    public class FirstModalViewModelTests
    {
        [Fact]
        public void PushModel_SecondModalViewModel()
        {
            var viewNaviagtionService = Substitute.For<INa>();
            var firstViewModel = new FirstModalViewModel(viewNaviagtionService);
            firstViewModel.OpenModal.Execute().Subscribe();
            viewNaviagtionService.Received().PushModal(Arg.Any<SecondModalViewModel>());
        }

        [Fact]
        public void PopModel_FirstModalViewModel()
        {
            var viewNaviagtionService = Substitute.For<IViewStackService>();
            var firstViewModel = new FirstModalViewModel(viewNaviagtionService);
            firstViewModel.PopModal.Execute().Subscribe();
            viewNaviagtionService.Received().PopModal();
        }
    }
}
