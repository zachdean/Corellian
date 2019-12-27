using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Reactive;
using System.Text;
using NSubstitute;
using Xunit;
using System.Collections.Immutable;
using CorellianSample;

namespace CorellianSampleTests
{
    public class NavigationServiceTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PushModal_ExpectSuccess(bool withNavigationPage)
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService<ITestInterface>().Returns(new TestClass());

            INavigationService navigationService = new NavigationService(viewStackService, serviceProvider);

            navigationService.PushModal<ITestInterface>(withNavigationPage);

            serviceProvider.Received().GetService<ITestInterface>();
            viewStackService.Received().PushModal(Arg.Any<TestClass>(), withNavigationPage: withNavigationPage);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        [InlineData(true, false)]
        public void PushPage_ExpectSuccess(bool resetStack, bool animate)
        {
            var viewStackService = Substitute.For<IViewStackService>();
            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService<ITestInterface>().Returns(new TestClass());

            INavigationService navigationService = new NavigationService(viewStackService, serviceProvider);

            navigationService.PushPage<ITestInterface>(resetStack, animate);

            serviceProvider.Received().GetService<ITestInterface>();
            viewStackService.Received().PushPage(Arg.Any<TestClass>(), resetStack: resetStack, animate: animate);
        }

        private interface ITestInterface : IViewModel { }
        private class TestClass : ITestInterface
        {
            public string Id => throw new NotImplementedException();
        }
    }    
}
