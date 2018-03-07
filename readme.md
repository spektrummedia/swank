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
xmlns:swank="clr-namespace:Plugin.Swank;assembly=Swank.FormsPlugin"
```

Then add the xaml:

```xml
<!-- ViewerImages = IEnumerable<ViewerImage> -->
<swank:Viewer ItemsSource="{Binding ViewerImages}" />
```