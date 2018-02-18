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
            MainPage = new NavigationPage(new ContentPage
            {
                Title = "Swank - iOS",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Viewer
                        {
                            ItemsSource = new List<ViewerImage>
                            {
                                new ViewerImage {Source = ImageSource.FromUri(new Uri("http://via.placeholder.com/400x300"))}
                            }
                        }
                    }
                }
            });
        }
    }
}