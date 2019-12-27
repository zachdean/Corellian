using NSubstitute;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
using FluentAssertions;
using System.Collections.Immutable;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Corellian.Core.Services;
using Splat;
using System.Linq;

namespace Corellian.Test
{
    public class NavigationServiceTest
    {
        private readonly IView view;
        private readonly IServiceProvider serviceProvider;
        private readonly INavigationService navigationService;
        private readonly TestClass viewModel = new TestClass();
        private readonly TestArgsClass paramterViewModel = new TestArgsClass();
        private readonly Subject<IViewModel> PagePoppedSubject = new Subject<IViewModel>();

        public NavigationServiceTest()
        {
            view = Substitute.For<IView>();
            view.PagePopped.Returns(PagePoppedSubject);

            serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService<ITestInterface>().Returns(viewModel);
            serviceProvider.GetService<ISecondTestInterface>().Returns(Substitute.For<ISecondTestInterface>());
            serviceProvider.GetService<ITestArgs>().Returns(paramterViewModel);

            navigationService = new NavigationService(view, serviceProvider, Substitute.For<IFullLogger>());
        }


        #region Stacks
        /// <summary>
        /// Tests to verify the return value is an observable type.
        /// </summary>
        [Fact]
        public void PageStack_ExpectToNotBeBehavior()
        {
            navigationService.PageStack.Should().NotBeAssignableTo<BehaviorSubject<IImmutableList<IViewModel>>>();
        }

        /// <summary>
        /// Tests to verify the return value is not a subject type.
        /// </summary>
        [Fact]
        public void ModalStack_ExpectToNotBeBehavior()
        {
            navigationService.ModalStack.Should().NotBeAssignableTo<BehaviorSubject<IImmutableList<IViewModel>>>();
        }

        #endregion

        #region Pop Modal

        /// <summary>
        /// Checks to make sure that the pop modal method works correctly.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Pop_Modal()
        {
            await navigationService.PushModal<ITestInterface>();

            // When
            var item = await navigationService.ModalStack.FirstAsync();
            item.Should().NotBeEmpty();
            await navigationService.PopModal();

            // Then
            item = await navigationService.ModalStack.FirstAsync();
            item.Should().BeEmpty();
        }

        /// <summary>
        /// Checks to make sure that the pop modal observables are received.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Receive_Pop_Modal()
        {
            // Given, When
            await navigationService.PushModal<ITestInterface>();

            // When
            await navigationService.PopModal();

            // Then
            await view.Received().PopModal();
        }

        /// <summary>
        /// Checks to make sure that there is a exception thrown if the stack happens to be empty.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Throw_If_Model_Stack_Empty()
        {

            // When
            var result = await Record.ExceptionAsync(async () => await navigationService.PopModal()).ConfigureAwait(false);

            // Then
            result.Should().BeOfType<InvalidOperationException>();
            result?.Message.Should().Be("Stack is empty.");
        }
        #endregion

        /// <summary>
        /// Tests associated with the pop page methods.
        /// </summary>
        #region The Pop Page Method

        //// <summary>
        /// Checks to make sure that the pop Page method works correctly.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Pop_Page_ViewRecievedCall()
        {


            await navigationService.PushPage<ITestInterface>();

            // When
            var item = await navigationService.PageStack.FirstAsync();
            item.Should().NotBeEmpty();
            await navigationService.PopPage();

            await view.Received().PopPage();
        }

        [Fact]
        public async Task PagePopped_RemovedFromStack()
        {
            await navigationService.PushPage<ITestInterface>();

            var item = await navigationService.PageStack.FirstAsync();
            item.Should().NotBeEmpty();

            PagePoppedSubject.OnNext(viewModel);

            // Then
            item = await navigationService.PageStack.FirstAsync();
            item.Should().BeEmpty();
        }

        /// <summary>
        /// Checks to make sure that the pop Page observables are received.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Receive_Pop_Page()
        {
            // Given, When
            await navigationService.PushPage<ITestInterface>();

            // When
            await navigationService.PopPage();

            // Then
            await view.Received().PopPage();
        }

        #endregion

        /// <summary>
        /// Tests for the pop to root method.
        /// </summary>
        #region The Pop To Root Page Method

        /// <summary>
        /// Tests to verify that no exception is thrown if the stack happens to be empty.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task PopToRoot_Should_Throw_If_Page_Stack_Empty()
        {
            // When
            var result = await Record.ExceptionAsync(async () => await navigationService.PopToRootPage()).ConfigureAwait(false);

            // Then
            result.Should().BeOfType<InvalidOperationException>();
            result.Message.Should().Be("Stack is empty.");
        }

        /// <summary>
        /// Tests to verify the navigatino stack is cleared.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Clear_Navigation_Stack()
        {
            // Given
            for (int i = 0; i < 3; i++)
            {
                await navigationService.PushPage<ITestInterface>();
            }

            // When
            await navigationService.PopToRootPage();
            var result = await navigationService.PageStack.FirstOrDefaultAsync();

            // Then
            result.Should().ContainSingle();
        }

        #endregion

        #region The Push Modal Method

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PushModal_ExpectSuccess(bool withNavigationPage)
        {
            navigationService.PushModal<ITestInterface>(withNavigationPage);

            serviceProvider.Received().GetService<ITestInterface>();
            view.Received().PushModal(Arg.Any<TestClass>(), null, withNavigationPage);
        }

        [Fact]
        public void PushModal_WithArgs_ExpectSuccess()
        {
            var args = Enumerable.Range(1, 10).Cast<object>().ToArray();

            navigationService.PushModal<ITestArgs>(args: args);

            paramterViewModel.Args.Should().BeEquivalentTo(args);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        [InlineData(true, false)]
        public void PushPage_ExpectSuccess(bool resetStack, bool animate)
        {
            navigationService.PushPage<ITestInterface>(resetStack, animate);

            serviceProvider.Received().GetService<ITestInterface>();
            view.Received().PushPage(Arg.Any<TestClass>(), null, resetStack, animate);
        }

        [Fact]
        public void PushPage_WithArgs_ExpectSuccess()
        {
            var args = Enumerable.Range(1, 10).Cast<object>().ToArray();
                        
            navigationService.PushPage<ITestArgs>(args: args);

            paramterViewModel.Args.Should().BeEquivalentTo(args);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PushPage_WithArgsAndReset_ExpectSuccess(bool resetStack)
        {
            var args = Enumerable.Range(1, 10).Cast<object>().ToArray();

            navigationService.PushPage<ITestArgs>(resetStack,args: args);

            paramterViewModel.Args.Should().BeEquivalentTo(args);
        }


        /// <summary>
        /// Makes sure that the push and pop methods work correctly.
        /// </summary>
        /// <param name="amount">The number of pages.</param>
        /// <returns>A completion notification.</returns>
        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public async Task Should_Push_And_Pop(int amount)
        {
            // Given
            for (int i = 0; i < amount; i++)
            {
                await navigationService.PushModal<ITestInterface>();
            }

            navigationService.ModalStack.FirstAsync().Wait().Count.Should().Be(amount);
            for (int i = 0; i < amount; i++)
            {
                await navigationService.PopModal();
            }

            // When
            var result = await navigationService.ModalStack.FirstAsync();

            // Then
            result.Should().BeEmpty();
        }

        /// <summary>
        /// Tests to make sure that the push modal works.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Push_Modal()
        {
            // Given
            await navigationService.PushModal<ITestInterface>();

            // When
            var result = await navigationService.TopModal();

            // Then
            result.Should().NotBeNull();
            result.Should().Be(viewModel);
        }

        /// <summary>
        /// Tests to make sure that the push modal respects navigation.
        /// </summary>
        /// <param name="withNavigationPage">Whether to use a navigation page.</param>
        /// <returns>A completion notification.</returns>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Should_Push_Modal_Navigation_Page(bool withNavigationPage)
        {
            // When
            await navigationService.PushModal<ITestInterface>(withNavigationPage);

            // Then
            await view.Received().PushModal(Arg.Any<ITestInterface>(), null, withNavigationPage);
        }

        /// <summary>
        /// Tests to make sure we can push a page onto the stack.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Push_Page_On_Stack()
        {
            // When
            await navigationService.PushModal<ITestInterface>();
            var result = await navigationService.ModalStack.FirstAsync();

            // Then
            result.Should().NotBeEmpty();
            result.Should().ContainSingle();
        }

        /// <summary>
        /// Tests to make sure we receive an push modal notifications.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Receive_Push_Modal()
        {
            // When
            await navigationService.PushModal<ITestInterface>();

            // Then
            await view.Received().PushModal(Arg.Any<IViewModel>(), Arg.Any<string>());
        }

        /// <summary>
        /// Tests to make sure that we get an exception throw if we pass in a null view model.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task PushModal_Should_Throw_If_View_Model_Null()
        {
            // When
            var result = await Record.ExceptionAsync(async () => await navigationService.PushModal<IViewModel>()).ConfigureAwait(false);

            // Then
            result.Should().BeOfType<InvalidOperationException>();
        }
        #endregion

        /// <summary>
        /// Tests associated with the push page method.
        /// </summary>
        #region The Push Page Method

        /// <summary>
        /// Tests to make sure that we get an exception throw if we pass in a null view model.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task PushPage_Should_Throw_If_View_Model_Null()
        {
            // When
            var result = await Record.ExceptionAsync(async () => await navigationService.PushPage<IViewModel>()).ConfigureAwait(false);

            // Then
            result.Should().BeOfType<InvalidOperationException>();
        }

        /// <summary>
        /// Tests to make sure that the push page works.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Push_Page()
        {
            // When
            await navigationService.PushPage<ITestInterface>();
            var result = await navigationService.TopPage();

            // Then
            result.Should().NotBeNull();
            result.Should().Be(viewModel);
        }

        /// <summary>
        /// Tests to make sure we receive an push page notifications.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Receive_Push_Page()
        {
            // When
            await navigationService.PushPage<ITestInterface>();

            // Then
            await view.Received().PushPage(Arg.Any<IViewModel>(), null, false, true);
        }

        /// <summary>
        /// Tests to make sure we receive an push page notifications.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Clear_Navigation_Stack_If_Reset()
        {
            // When
            for (var i = 0; i < 3; i++)
            {
                await navigationService.PushPage<ITestInterface>();
            }
            await navigationService.PushPage<ITestInterface>(resetStack: true);
            var result = await navigationService.PageStack.FirstOrDefaultAsync();

            // Then
            result.Should().ContainSingle();
        }
        #endregion

        /// <summary>
        /// Tests for the TopModal method.
        /// </summary>
        #region The Top Modal Method
        /// <summary>
        /// Tests to make sure that it does not pop the stack.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task TopModal_Should_Not_Pop()
        {
            await navigationService.PushModal<ITestInterface>();

            // When
            await navigationService.TopModal();

            // Then
            await view.DidNotReceive().PopModal();
        }

        /// <summary>
        /// Tests to make sure that it returns the last element only.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task TopModal_Should_Return_Last_Element()
        {
            await navigationService.PushModal<ITestInterface>();
            await navigationService.PushModal<ISecondTestInterface>();

            // When
            var result = await navigationService.TopModal();

            // Then
            result.Should().BeAssignableTo<ISecondTestInterface>();
        }

        /// <summary>
        /// Tests to make sure it throws an exception if the stack is empty.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task TopModal_Should_Throw_If_Stack_Empty()
        {
            // When
            var result = await Record.ExceptionAsync(async () => await navigationService.TopModal()).ConfigureAwait(false);

            // Then
            result.Should().BeOfType<ArgumentOutOfRangeException>();
        }
        #endregion

        /// <summary>
        /// Tests for the TopPage method.
        /// </summary>
        #region The Top Page Method
        /// <summary>
        /// Tests to make sure that it does not pop the stack.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task TopPage_Should_Not_Pop()
        {
            await navigationService.PushPage<ITestInterface>();

            // When
            await navigationService.TopPage();

            // Then
            await view.DidNotReceive().PopPage();
        }

        /// <summary>
        /// Tests to make sure that it returns the last element only.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task TopPage_Should_Return_Last_Element()
        {
            await navigationService.PushPage<ITestInterface>();
            await navigationService.PushPage<ISecondTestInterface>();

            // When
            var result = await navigationService.TopPage();

            // Then
            result.Should().BeAssignableTo<ISecondTestInterface>();
        }

        /// <summary>
        /// Tests to make sure it throws an exception if the stack is empty.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task TopPage_Should_Throw_If_Stack_Empty()
        {
            // When
            var result = await Record.ExceptionAsync(async () => await navigationService.TopPage()).ConfigureAwait(false);

            // Then
            result.Should().BeOfType<ArgumentOutOfRangeException>();
        }
        #endregion


        #region Test Fixtures
        private interface ITestInterface : IViewModel { }
        public interface ISecondTestInterface : IViewModel { }
        private class TestClass : ITestInterface
        {
            public string Id => nameof(TestClass);
        }


        private interface ITestArgs : IViewModel, INavigateWithParamters { }
        private class TestArgsClass : ITestArgs
        {
            public string Id => nameof(TestArgsClass);

            public void Initialize(object[] args)
            {
                Args = args;
            }

            internal object[] Args { get; private set; }
        }


        #endregion
    }
}
