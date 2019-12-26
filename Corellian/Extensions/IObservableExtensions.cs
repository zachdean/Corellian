using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Reactive.Linq
{
    public static class IObservableExtensions
    {
        /// <summary>
        /// Subscribes to an Observable and provides default debugging in the case of an exception.
        /// It will provide the caller information as part of the logging.
        /// </summary>
        /// <typeparam name="T">The type of item signaled as part of the observable.</typeparam>
        /// <param name="observable">The observable to subscribe to.</param>
        /// <param name="callerMemberName">The name of the caller member.</param>
        /// <param name="callerFilePath">The file path of the caller member.</param>
        /// <param name="callerLineNumber">The line number of the caller member.</param>
        /// <returns>A disposable which when disposed will unsubscribe from the observable.</returns>
        public static IDisposable SubscribeSafe<T>(
            this IObservable<T> observable,
            [CallerMemberName]string callerMemberName = null,
            [CallerFilePath]string callerFilePath = null,
            [CallerLineNumber]int callerLineNumber = 0)
        {
            return observable
                .Subscribe(
                    _ => { },
                    ex =>
                    {
                        //var logger = new DefaultLogManager().GetLogger(typeof(SubscribeSafeExtensions));
                        //logger.Error(ex, "An exception went unhandled. Caller member name: '{0}', caller file path: '{1}', caller line number: {2}.", callerMemberName, callerFilePath, callerLineNumber);

                        Debugger.Break();
                    });
        }

        /// <summary>
        /// Adds a condition to the signaling of an observable which will not fire unless the value is not null.
        /// </summary>
        /// <typeparam name="T">The type of the observable.</typeparam>
        /// <param name="observable">The observable to add the condition to.</param>
        /// <returns>An observable which will not signal unless the value is not null.</returns>
        public static IObservable<T> WhereNotNull<T>(this IObservable<T> observable)
        {
            return observable.Where(x => x != null);
        }

        /// <summary>
        /// Will convert an observable so that it's value is ignored and converted into just returning <see cref="Unit"/>.
        /// This allows us just to be notified when the observable signals.
        /// </summary>
        /// <typeparam name="T">The current type of the observable.</typeparam>
        /// <param name="observable">The observable to convert.</param>
        /// <returns>The converted observable.</returns>
        public static IObservable<Unit> ToSignal<T>(this IObservable<T> observable)
        {
            if (observable == null)
            {
                throw new ArgumentNullException(nameof(observable));
            }

            return observable.Select(_ => Unit.Default);
        }
    }
}
