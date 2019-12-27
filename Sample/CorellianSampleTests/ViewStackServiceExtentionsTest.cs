using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using Corellian;
using CorellianSample;
using Xunit;

namespace CorellianSampleTests
{
    public class ViewStackServiceExtentionsTest
    {
        private interface ITestInterface : INavigable
        {

        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, false)]
        public void PushPage_ExpectedPushModelCalled(bool resetStack, bool animate)
        {
            var viewStack = Substitute.For<IViewStackService>();

            viewStack.PushPage<ITestInterface>(resetStack, animate);

            viewStack.Received().PushPage(Arg.Is<ViewModelTransfer>(x => x.ViewModel == typeof(ITestInterface)), null, resetStack, animate);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, false)]
        public void PushNavigablePage_ExpectedPushModelCalled(bool resetStack, bool animate)
        {
            var viewStack = Substitute.For<IViewStackService>();

            viewStack.PushNavigablePage<ITestInterface>(resetStack, animate);

            viewStack.Received().PushPage(Arg.Is<ViewModelTransfer>(x => x.ViewModel == typeof(ITestInterface)), null, resetStack, animate);
        }
    }
}
