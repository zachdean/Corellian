using Corellian.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ReactiveUI;
using System;
using Xunit;
using FluentAssertions;
using Splat;

namespace Corellian.Test
{
    public class IServiceCollectionExtensionsTest
    {
        [Fact]
        public void AddCorellianCore_ExpectRegistered()
        {
            var services = new ServiceCollection();

            services.AddSingleton(Substitute.For<IView>());
            services.AddSingleton(Substitute.For<IFullLogger>());

            services.AddCorellianCore();

            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<IViewLocator>().Should().BeOfType<Core.Services.ViewLocator>();
            provider.GetRequiredService<INavigationService>().Should().BeOfType<NavigationService>();

        }


        [Fact]
        public void AddCorellianCore_ThrowIfViewIsNotRegistered()
        {
            var services = new ServiceCollection();

            var resalt = Record.Exception(services.AddCorellianCore);

            resalt.Should().BeOfType<InvalidOperationException>();
        }
    }
}
