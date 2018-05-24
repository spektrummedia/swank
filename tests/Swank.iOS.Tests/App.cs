using System;
using System.Collections.Generic;
using Plugin.Swank;
using Plugin.Swank.Panorama;
using Plugin.Swank.Panorama.ImageSources;
using Swank.FormsPlugin;
using Xamarin.Forms;

namespace Swank.iOS.Tests
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            var gallery = new Gallery()
            {
                ItemsSource = new List<ViewerImage>
                {
                    new ViewerImage {Source = ImageSource.FromUri(new Uri("http://via.placeholder.com/1700x1275"))},
                    new ViewerImage
                    {
                        FilePath = "360-panorama-matador-seo.jpg",
                        Source = ImageSource.FromFile("360-panorama-matador-seo.jpg"),
                        Is360 = true,
                        Toggle360ModeText = "Trigger immersion"
                    },
                    new ViewerImage {Source = ImageSource.FromUri(new Uri("http://via.placeholder.com/400x300"))},
                    new ViewerImage
                    {
                        FilePath = "https://d36tnp772eyphs.cloudfront.net/blogs/1/2006/11/360-panorama-matador-seo.jpg",
                        Source = ImageSource.FromUri(new Uri(
                            "https://d36tnp772eyphs.cloudfront.net/blogs/1/2006/11/360-panorama-matador-seo.jpg")),
                        Is360 = true,
                        Toggle360ModeText = "Trigger immersion"
                    },
                    new ViewerImage {Source = ImageSource.FromUri(new Uri("http://via.placeholder.com/900x100"))}
                },
            };

            gallery.PositionSelected += (sender, args) =>
            {
                Console.WriteLine(args.NewValue);
            };

            MainPage = new ContentPage
            {
                Title = "Swank - iOS",
                Content = new StackLayout()
                {
                    Spacing = 0,
                    Children =
                    {
                        gallery
                    }
                }
            };
        }
    }
}