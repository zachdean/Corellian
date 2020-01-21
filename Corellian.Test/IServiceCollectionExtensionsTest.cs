using Corellian.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ReactiveUI;
using System;
using Xunit;
using FluentAssertions;
using Splat;
using System.Linq;
using FluentAssertions.Microsoft.Extensions.DependencyInjection;

namespace Corellian.Test
{
    public class IServiceCollectionExtensionsTest
    {
        private readonly IServiceCollection services;

        public IServiceCollectionExtensionsTest()
        {
            services = new ServiceCollection();
        }

        [Fact]
        public void AddCorellianCore_ExpectRegistered()
        {
            services.AddSingleton(Substitute.For<IView>());
            services.AddSingleton(Substitute.For<IFullLogger>());

            services.AddCorellianCore();

            services.Should()
                .HaveService<IViewLocator>()
                .AsSingleton();
            services.Should()
                .HaveService<INavigationService>()
                .AsSingleton();

            services.Should().HaveCount(4);
        }


        [Fact]
        public void AddCorellianCore_ThrowIfViewIsNotRegistered()
        {
            var resalt = Record.Exception(services.AddCorellianCore);

            resalt.Should().BeOfType<InvalidOperationException>();
        }

        [Fact]
        public void AddView_RegisteresView()
        {
            services.AddView<ITestViewModel, TestView>();

            services.Should()
                .HaveService<IViewFor<ITestViewModel>>()
                .AsTransient()
                .And
                .HaveCount(1);
        }

        public interface ITestViewModel : IViewModel
        {
        }

        public class TestView : IViewFor<ITestViewModel>
        {
            public ITestViewModel ViewModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            object IViewFor.ViewModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        }

    }

    
}
