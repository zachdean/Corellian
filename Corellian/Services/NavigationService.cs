using Microsoft.Extensions.DependencyInjection;
using Splat;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;


[assembly: InternalsVisibleTo("Corellian.Test")]
[assembly: InternalsVisibleTo("Corellian.Xamarin")]
namespace Corellian.Core.Services
{
    internal class NavigationService : INavigationService, IDisposable
    {
        private readonly IFullLogger logger;
        private readonly IServiceProvider serviceProvider;
        /// <summary>
        /// Gets the modal subject.
        /// </summary>
        internal readonly BehaviorSubject<IImmutableList<IViewModel>> ModalSubject;

        /// <summary>
        /// Gets the page subject.
        /// </summary>
        internal readonly BehaviorSubject<IImmutableList<IViewModel>> PageSubject;
        /// <summary>
        /// Gets the modal navigation stack.
        /// </summary>
        public IObservable<IImmutableList<IViewModel>> ModalStack => ModalSubject.AsObservable();

        /// <summary>
        /// Gets the page navigation stack.
        /// </summary>
        public IObservable<IImmutableList<IViewModel>> PageStack => PageSubject.AsObservable();

        /// <summary>
        /// Pops the <see cref="INavigable" /> off the stack.
        /// </summary>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns>An observable that signals when the pop is complete.</returns>
        public IObservable<Unit> PopModal(bool animate = true) => View.PopModal().Do(_ => PopStackAndTick(ModalSubject));

        /// <inheritdoc />
        public IObservable<Unit> PopPage(bool animate = true) => View.PopPage(animate);

        /// <inheritdoc />
        public IObservable<Unit> PopToRootPage(bool animate = true) => View.PopToRootPage(animate).Do(_ => PopRootAndTick(PageSubject));

        public IView View { get; }
       

        public IObservable<Unit> PushModal<TViewModel>(INavigationParameter parameter = null, bool withNavigationPage = true) where TViewModel : IViewModel
        {
            TViewModel viewModel = ResolveViewModel<TViewModel>();

            if (viewModel is INavigatable paramViewModel)
            {
                paramViewModel.WhenNavigatingTo(parameter);
            }

            return View
                .PushModal(viewModel, null, withNavigationPage)
                .Do(_ =>
                {
                    AddToStackAndTick(ModalSubject, viewModel, false);
                    logger.Debug("Added modal '{modal.Id}' (contract '{contract}') to stack.");
                });
        }

        public IObservable<Unit> PushPage<TViewModel>(INavigationParameter parameter = null, bool resetStack = false, bool animate = true) where TViewModel : IViewModel
        {
            TViewModel viewModel = ResolveViewModel<TViewModel>();

            if (viewModel is INavigatable paramViewModel)
            {
                paramViewModel.WhenNavigatingTo(parameter);
            }

            return View
                .PushPage(viewModel, null, resetStack, animate)
                .Do(_ =>
                {
                    AddToStackAndTick(PageSubject, viewModel, resetStack);
                    logger.Debug($"Added page '{viewModel.Id}' to stack.");
                });

            
        }

        /// <summary>
        /// Returns the top modal from the current modal stack.
        /// </summary>
        /// <returns>An observable that signals the top modal view model.</returns>
        public IObservable<IViewModel> TopModal() => ModalSubject.FirstAsync().Select(x => x[x.Count - 1]);

        /// <summary>
        /// Returns the top page from the current navigation stack.
        /// </summary>
        /// <returns>An observable that signals the top page view model.</returns>
        public IObservable<IViewModel> TopPage() => PageSubject.FirstAsync().Select(x => x[x.Count - 1]);


        public NavigationService(IView view, IServiceProvider serviceProvider, IFullLogger logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            View = view;
            ModalSubject = new BehaviorSubject<IImmutableList<IViewModel>>(ImmutableList<IViewModel>.Empty);
            PageSubject = new BehaviorSubject<IImmutableList<IViewModel>>(ImmutableList<IViewModel>.Empty);

            View.PagePopped.Do(poppedPage =>
            {
                var currentPageStack = PageSubject.Value;
                if (currentPageStack.Count > 0 && poppedPage == currentPageStack[currentPageStack.Count - 1])
                {
                    var removedPage = PopStackAndTick(PageSubject);
                    this.logger.Debug("Removed page '{0}' from stack.", removedPage.Id);
                }
            }).SubscribeSafe();
        }
        private TViewModel ResolveViewModel<TViewModel>() => serviceProvider.GetRequiredService<TViewModel>();
        /// <inheritdoc />
        public void Dispose()
        {
            ModalSubject?.Dispose();
            PageSubject?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Adds to stack and tick.
        /// </summary>
        /// <typeparam name="T">The view model type.</typeparam>
        /// <param name="stackSubject">The stack subject.</param>
        /// <param name="item">The item.</param>
        /// <param name="reset">if set to <c>true</c> [reset].</param>
        private static void AddToStackAndTick<T>(BehaviorSubject<IImmutableList<T>> stackSubject, T item, bool reset)
        {
            var stack = stackSubject.Value;

            if (reset)
            {
                stack = new[] { item }.ToImmutableList();
            }
            else
            {
                stack = stack.Add(item);
            }

            stackSubject.OnNext(stack);
        }

        /// <summary>
        /// Pops the stack and notifies observers.
        /// </summary>
        /// <typeparam name="T">The view model type.</typeparam>
        /// <param name="stackSubject">The stack subject.</param>
        /// <returns>The view model popped.</returns>
        /// <exception cref="InvalidOperationException">Stack is empty.</exception>
        private static T PopStackAndTick<T>(BehaviorSubject<IImmutableList<T>> stackSubject)
        {
            var stack = stackSubject.Value;

            if (stack.Count == 0)
            {
                throw new InvalidOperationException("Stack is empty.");
            }

            var removedItem = stack[stack.Count - 1];
            stack = stack.RemoveAt(stack.Count - 1);
            stackSubject.OnNext(stack);
            return removedItem;
        }

        /// <summary>
        /// Pops the root and notifies observers.
        /// </summary>
        /// <typeparam name="T">The view model type.</typeparam>
        /// <param name="stackSubject">The stack subject.</param>
        /// <exception cref="System.InvalidOperationException">Stack is empty.</exception>
        protected static void PopRootAndTick<T>(BehaviorSubject<IImmutableList<T>> stackSubject)
        {
            IImmutableList<T> poppedStack = ImmutableList<T>.Empty;

            if (stackSubject?.Value == null || !stackSubject.Value.Any())
            {
                throw new InvalidOperationException("Stack is empty.");
            }

            stackSubject
                .Take(stackSubject.Value.Count - 1)
                .Where(stack => stack != null)
                .Subscribe(stack => poppedStack = stack.RemoveRange(stack.IndexOf(stack[0]), stack.Count - 1));

            stackSubject.OnNext(poppedStack);
        }
    }
}
