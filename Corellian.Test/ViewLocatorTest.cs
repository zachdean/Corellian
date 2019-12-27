using FluentAssertions;
using NSubstitute;
using System;
using Xunit;
using ReactiveUI;

namespace Corellian.Test
{
    public class ViewLocatorTest
    {
        private readonly IServiceProvider serviceProvider;
        public readonly IViewLocator viewLocator;
        private readonly IViewFor<TestClass> testView;

        public ViewLocatorTest()
        {
            serviceProvider = Substitute.For<IServiceProvider>();
            testView = Substitute.For<IViewFor<TestClass>>();
            viewLocator = new Core.Services.ViewLocator(serviceProvider);
        }

        [Fact]
        public void ResolveView_ViewModel_ReturnsIVeiwForViewModel()
        {
            var testClass = new TestClass();

            serviceProvider.GetService(typeof(IViewFor<TestClass>)).Returns(testView);

            var view = viewLocator.ResolveView(testClass);

            serviceProvider.Received().GetService(typeof(IViewFor<TestClass>));            

            view.Should().Be(testView);
        }

        [Fact]
        public void ResolveView_LocatableViewModel_ReturnsIVeiwForViewModel()
        {
            var testClass = new TestLocatableClass();
            
            serviceProvider.GetService(typeof(IViewFor<TestClass>)).Returns(testView);

            var view = viewLocator.ResolveView(testClass);

            serviceProvider.Received().GetService(typeof(IViewFor<TestClass>));            

            view.Should().Be(testView);
        }

        public class TestClass
        {            
        }

        public class TestLocatableClass : ILocatable
        {
            public Type ViewModelInterface => typeof(TestClass);

            public string Id => throw new NotImplementedException();
        }
    }
}
