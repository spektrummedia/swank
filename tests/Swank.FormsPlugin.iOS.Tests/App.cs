using System;
using System.Collections.Generic;
using Swank.FormsPlugin.Abstractions;
using Xamarin.Forms;

namespace Swank.FormsPlugin.iOS.Tests
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            var viewer = new Viewer
            {
                ItemsSource = new List<ViewerImage>
                {
                    new ViewerImage {Source = ImageSource.FromUri(new Uri("http://via.placeholder.com/350x150"))}
                }
            };


            MainPage = new NavigationPage(new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Padding = new Thickness(32, 32, 32, 32),
                    Children =
                    {
                        new Label {Text = "Swank - iOS"},
                        viewer
                    }
                }
            });
        }
    }
}