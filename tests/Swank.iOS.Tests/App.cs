using System;
using System.Collections.Generic;
using Plugin.Swank;
using Xamarin.Forms;

namespace Swank.iOS.Tests
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            // Classic viewer
            var viewer = new Viewer
            {
                ItemsSource = new List<ViewerImage>
                {
                    //new ViewerImage {Source = ImageSource.FromUri(new Uri("http://via.placeholder.com/1700x1275"))},
                    new ViewerImage {Source = ImageSource.FromUri(new Uri("https://farm1.static.flickr.com/853/40828093624_702a28cbf6_f.jpg")), Is360 = true},
                    //new ViewerImage {Source = ImageSource.FromUri(new Uri("http://via.placeholder.com/400x300"))},
                    //new ViewerImage {Source = ImageSource.FromUri(new Uri("http://via.placeholder.com/900x100"))}
                }
            };

            MainPage = new ContentPage
            {
                Title = "Swank - iOS",
                Content = new StackLayout()
                {
                    Spacing = 0,
                    Children =
                    {
                        viewer
                    }
                }
            };
        }
    }
}