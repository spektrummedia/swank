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
                    new ViewerImage {Source = ImageSource.FromUri(new Uri("http://via.placeholder.com/800x300"))},
                    new ViewerImage {Source = ImageSource.FromUri(new Uri("http://www.davidjmulder.com/wp-content/uploads/2014/07/Enq-360.jpg")), Is360 = true},
                    new ViewerImage {Source = ImageSource.FromUri(new Uri("http://via.placeholder.com/400x300"))}
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