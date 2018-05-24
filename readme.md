<p align="left"><img src="logo/vertical.png" alt="swank" height="200px"></p>

# Swank Plugin for Xamarin.Forms

[![Build status](https://ci.appveyor.com/api/projects/status/bu21xmnmadm0crsn?svg=true)](https://ci.appveyor.com/project/spektrum/swank)
[![NuGet](https://img.shields.io/nuget/dt/Swank.FormsPlugin.svg)](https://www.nuget.org/packages/Swank.FormsPlugin/)

Xamarin embeddable image viewer with 360 degree support

### iOS

In your iOS project call:

```
SwankImplementation.Init();
```


### XAML

First add the xmlns namespace:

```
xmlns:swankPlugin="clr-namespace:Swank.FormsPlugin;assembly=Swank.FormsPlugin"
```

Then add the xaml:

```xml
<!-- ViewerImages = IEnumerable<ViewerImage> -->
<swankPlugin:Gallery ItemsSource="{Binding ViewerImages}" />
```

#### Options

`PositionSelected`: You can attach an event handler that will be fired with the current carousel position.

```xml
<swankPlugin:Gallery PositionSelected="Carousel_OnPositionSelected" />
```

```csharp
private void Carousel_OnPositionSelected(object sender, PositionSelectedEventArgs e)
{
    Console.WriteLine($"Index is {e.NewValue}")
}
```