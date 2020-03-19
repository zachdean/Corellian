using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Corellian.Xamarin
{
    /// <summary>
    /// The main navigation view.
    /// </summary>
    public class NavigationServicePage : NavigationPage, IView, IEnableLogger
    {
        private readonly IScheduler _backgroundScheduler;
        private readonly IScheduler _mainScheduler;
        private readonly IViewLocator _viewLocator;
        private readonly IFullLogger _logger;

        public NavigationServicePage(IScheduler mainScheduler, IScheduler backgroundScheduler, IViewLocator viewLocator, Page page) : base(page)
        {
            _backgroundScheduler = backgroundScheduler;
            _mainScheduler = mainScheduler;
            _viewLocator = viewLocator;

            PagePopped = Observable
                .FromEventPattern<NavigationEventArgs>(x => Popped += x, x => Popped -= x)
                .Select(ep => ep.EventArgs.Page.BindingContext as IViewModel)
                .WhereNotNull();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationServicePage"/> class.
        /// </summary>
        /// <param name="mainScheduler">The main scheduler to scheduler UI tasks on.</param>
        /// <param name="backgroundScheduler">The background scheduler.</param>
        /// <param name="viewLocator">The view locator which will find views associated with view models.</param>
        public NavigationServicePage(IScheduler mainScheduler, IScheduler backgroundScheduler, IViewLocator viewLocator)
        {
            _backgroundScheduler = backgroundScheduler;
            _mainScheduler = mainScheduler;
            _viewLocator = viewLocator;

            PagePopped = Observable
                .FromEventPattern<NavigationEventArgs>(x => Popped += x, x => Popped -= x)
                .Select(ep => ep.EventArgs.Page.BindingContext as IViewModel)
                .WhereNotNull();
        }

        /// <inheritdoc />
        public IScheduler MainThreadScheduler => _mainScheduler;

        /// <inheritdoc />
        public IObservable<IViewModel> PagePopped { get; }

        /// <inheritdoc />
        public IObservable<Unit> PopModal() =>
            Navigation
                .PopModalAsync()
                .ToObservable()
                .ToSignal()
                .ObserveOn(_mainScheduler); // XF completes the pop operation on a background thread :/

        /// <inheritdoc />
        public IObservable<Unit> PopPage(bool animate) =>
            Navigation
                .PopAsync(animate)
                .ToObservable()
                .ToSignal()
                .ObserveOn(_mainScheduler); // XF completes the pop operation on a background thread :/

        /// <inheritdoc />
        public IObservable<Unit> PopToRootPage(bool animate) =>
             Navigation
                .PopToRootAsync(animate)
                .ToObservable()
                .ToSignal()
                .ObserveOn(_mainScheduler);

        /// <inheritdoc />
        public IObservable<Unit> PushModal(IViewModel modalViewModel, bool withNavigationPage = true) =>
            Observable
                .Start(
                    () =>
                    {
                        var page = LocatePageFor(modalViewModel);
                        SetPageTitle(page, modalViewModel.Id);
                        if (withNavigationPage)
                        {
                            return new NavigationServicePage(_mainScheduler,_backgroundScheduler,_viewLocator,page);
                        }

                        return page;
                    },
                    CurrentThreadScheduler.Instance)
                .ObserveOn(_mainScheduler)
                .SelectMany(
                    page => { MainThread.BeginInvokeOnMainThread(async () =>
                     {
                         await Navigation
                                 .PushModalAsync(page);
                     });
                        return Observable.Return(Unit.Default);
                    });

        /// <inheritdoc />
        public IObservable<Unit> PushPage(
            IViewModel viewModel,
            bool resetStack,
            bool animate) =>
            Observable
                .Start(() => LocatePageFor(viewModel), CurrentThreadScheduler.Instance)
                .ObserveOn(_mainScheduler)
                .SelectMany(page => PushPageOnNavigation(resetStack, animate, page));

        private IObservable<Unit> PushPageOnNavigation(bool resetStack, bool animate, Page page)
        {
            if (resetStack)
            {
                if (Navigation.NavigationStack.Count == 0)
                {
                    return Navigation.PushAsync(page, false).ToObservable();
                }

                // XF does not allow us to pop to a new root page. Instead, we need to inject the new root page and then pop to it.
                Navigation
                    .InsertPageBefore(page, Navigation.NavigationStack[0]);

                return Navigation
                    .PopToRootAsync(false)
                    .ToObservable();
            }

            return Navigation
                .PushAsync(page, animate)
                .ToObservable();
        }

        private IView LocateNavigationFor(IViewModel viewModel)
        {
            var view = _viewLocator.ResolveView(viewModel);
            var navigationPage = view as IView;

            if (navigationPage is null)
            {
                _logger.Debug($"No navigation view could be located for type '{viewModel.GetType().FullName}', using the default navigation page.");
                navigationPage = Locator.Current.GetService<IView>(nameof(NavigationServicePage)) ?? Locator.Current.GetService<IView>();
            }

            return navigationPage;
        }

        private Page LocatePageFor(IViewModel viewModel)
        {
            var view = _viewLocator.ResolveView(viewModel);
            var page = view as Page;

            if (view == null)
            {
                throw new InvalidOperationException($"No view could be located for type '{viewModel.GetType().FullName}'. Be sure Splat has an appropriate registration.");
            }

            if (view == null)
            {
                throw new InvalidOperationException($"Resolved view '{view.GetType().FullName}' for type '{viewModel.GetType().FullName}'' does not implement IViewFor.");
            }

            if (page == null)
            {
                throw new InvalidOperationException($"Resolved view '{view.GetType().FullName}' for type '{viewModel.GetType().FullName}' is not a Page.");
            }

            view.ViewModel = viewModel;
            SetPageTitle(page, viewModel.Id);
            return page;
        }

        private void SetPageTitle(Page page, string resourceKey)
        {
            // var title = Localize.GetString(resourceKey);
            // TODO: ensure resourceKey isn't null and is localized.
            page.Title = resourceKey;
        }
    }
}
