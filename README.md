# Corellian
Corellian is a ViewModel First Navigation library that does navigation in a reactive way. This Library has its roots from a 
very good navigation library built on [ReactiveUI](https://reactiveui.net/) called [Sextant](https://github.com/reactiveui/Sextant).
This project was pretty close to what I wanted out of a navigation library, however, there was a couple of unique requirements
that caused enough of a difference to create theis fork. The primary motivation behind this library is to provide a easy way to 
navigate from a interface instead of a conrete implementation and have the view model and view location handled with [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/).

## Register Dependencies

to use Corellian simply call AddCorrellian to during your container set up. This method will handle all of the set up required to use [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/)
with [ReactiveUI](https://reactiveui.net/) (change Splat container, register dependancies).

```
var services = new ServiceCollection();

//registers all of Corellian's dependancies
services.AddCorellian();
```

## ViewModels
View Models must implement IViewModel in order to be used with the navigation service.

## Views

All view must implement IViewFor<TViewModel>. ReactiveUI provides some [platform specific helper classes](https://reactiveui.net/docs/guidelines/platform/) the help out the boiler plate. Although
not required, all views should be registered for the the view model interface instead of the view model itself. Just make sure you register the 
view model in the contanter the same way you use it in the view. 

All Views need to be registered in the contianter. Corellian provides a usefull exentsion method to help with this. As a general practice
all views should be registered in a extension method to make the registerating cleaner.

```
public static IServiceCollection AddViews(this IServiceCollection services)
{
  // IHomeViewModel implements IViewModel, HomeView implements IViewFor<IHomeViewModel>
  services.AddView<IHomeViewModel, HomeView>();
  ...
}
```

