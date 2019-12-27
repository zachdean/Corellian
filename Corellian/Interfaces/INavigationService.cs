using System;
using System.Collections.Immutable;
using System.Reactive;

namespace Corellian
{
    public interface INavigationService
    {
        // Summary:
        //     Gets the modal navigation stack.
        IObservable<IImmutableList<IViewModel>> ModalStack { get; }
        //
        // Summary:
        //     Gets the page navigation stack.
        IObservable<IImmutableList<IViewModel>> PageStack { get; }
        //
        // Summary:
        //     Gets the current view on the stack.
        IView View { get; }

        //
        // Summary:
        //     Pops the Sextant.INavigable off the stack.
        //
        // Parameters:
        //   animate:
        //     if set to true [animate].
        //
        // Returns:
        //     An observable that signals when the pop has been completed.
        IObservable<Unit> PopModal(bool animate = true);
        //
        // Summary:
        //     Pops the Sextant.INavigable off the stack.
        //
        // Parameters:
        //   animate:
        //     if set to true [animate].
        //
        // Returns:
        //     An observable that signals when the pop has been completed.
        IObservable<Unit> PopPage(bool animate = true);
        //
        // Summary:
        //     Pops to root page.
        //
        // Parameters:
        //   animate:
        //     If set to true animate.
        //
        // Returns:
        //     An observable that signals when the pop has been completed.
        IObservable<Unit> PopToRootPage(bool animate = true);
        //
        // Summary:
        //     Pushes the Sextant.INavigable onto the stack.
        //
        // Parameters:
        //   contract:
        //     The contract.
        //
        //   withNavigationPage:
        //     Value indicating whether to wrap the modal in a navigation page.
        //
        // Type parameters:
        //   TViewModel:
        //     The type of the view model.
        //
        // Returns:
        //     An observable that signals when the push has been completed.
        IObservable<Unit> PushModal<TViewModel>(bool withNavigationPage = true, params object[] args) where TViewModel : IViewModel;
        //
        // Summary:
        //     Pushes the Sextant.INavigable onto the stack.
        //
        // Parameters:
        //   contract:
        //     The contract.
        //
        //   resetStack:
        //     if set to true [reset stack].
        //
        //   animate:
        //     if set to true [animate].
        //
        // Type parameters:
        //   TViewModel:
        //     The type of the view model.
        //
        // Returns:
        //     An observable that signals when the push has been completed.
        IObservable<Unit> PushPage<TViewModel>(bool resetStack = false, bool animate = true, params object[] args) where TViewModel : IViewModel;

        //
        // Summary:
        //     Returns the top modal from the current modal stack.
        //
        // Returns:
        //     An observable that signals the top modal of the stack.
        IObservable<IViewModel> TopModal();
        //
        // Summary:
        //     Returns the top page from the current navigation stack.
        //
        // Returns:
        //     An observable that signals the top page of the stack.
        IObservable<IViewModel> TopPage();
    }
}
