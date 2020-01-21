using CorellianSample;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using FluentAssertions.Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using CorellianSample.ViewModels;

namespace CorellianSampleTests
{
    public class ServicesExtensionsTest
    {
        private readonly IServiceCollection services = new ServiceCollection();

        [Fact]
        public void AddViews_EnsureRegistered()
        {
            services.AddViews();

            services.Should()
                .HaveService<IViewFor<IHomeViewModel>>()
                .AsTransient();

            services.Should()
                .HaveService<IViewFor<IRedViewModel>>()
                .AsTransient();

            services.Should()
                .HaveService<IViewFor<IFirstModalViewModel>>()
                .AsTransient();

            services.Should()
                .HaveService<IViewFor<ISecondModalViewModel>>()
                .AsTransient();

            services.Should()
                .HaveCount(4);
        }

        [Fact]
        public void AddViewModels_EnsureRegistered()
        {
            services.AddViewModels();

            services.Should()
                .HaveService<IHomeViewModel>()
                .AsTransient();

            services.Should()
                .HaveService<IRedViewModel>()
                .AsTransient();

            services.Should()
                .HaveService<IFirstModalViewModel>()
                .AsTransient();

            services.Should()
                .HaveService<ISecondModalViewModel>()
                .AsTransient();

            services.Should()
                .HaveCount(4);
        }

    }
}
