using Corellian.Extensions;
using Corellian.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;

namespace Corellian.Test
{
    public class IServiceCollectionExtensionsTest
    {
        [Fact]
        public void AddCorellianCore_ExpectRegistered()
        {
            var services = new ServiceCollection();

            services.AddCorellianCore();

            services.AddSingleton(Substitute.For<IView>());

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
