using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;

namespace Swank.FormsPlugin.Abstractions
{
    public class Viewer : ScrollView
    {
        public IList ItemsSource
        {
            get => (IList) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public DataTemplate ItemTemplate { get; set; }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public int SelectedIndex
        {
            get => (int) GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IList),
                typeof(Viewer),
                default(IList),
                BindingMode.OneWay,
                propertyChanged: (bindableObject, oldValue, newValue) =>
                {
                    ((Viewer) bindableObject).ItemsSourceChanged(bindableObject, (IList) oldValue, (IList) newValue);
                }
            );

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(
                nameof(SelectedItem),
                typeof(object),
                typeof(Viewer),
                null,
                BindingMode.OneWay,
                propertyChanged: (bindable, oldValue, newValue) => { ((Viewer) bindable).UpdateSelectedIndex(); }
            );

        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create(
                nameof(SelectedIndex),
                typeof(int),
                typeof(Viewer),
                0,
                BindingMode.TwoWay,
                propertyChanged: (bindable, oldValue, newValue) => { ((Viewer) bindable).UpdateSelectedItem(); }
            );

        private readonly StackLayout _imageStack;

        public Viewer()
        {
            // Boxing
            Padding = 0;
            Margin = 0;

            // Positioning
            Orientation = ScrollOrientation.Horizontal;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;

            _imageStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(0),
                Margin = new Thickness(0),
                Spacing = 0,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            ItemTemplate = new DataTemplate(() =>
            {
                var image = new Image();
                image.SetBinding(Image.SourceProperty, nameof(ViewerImage.Source));
                image.Aspect = Aspect.AspectFit;

                var layout = new StackLayout();
                layout.Children.Add(image);

                return layout;
            });

            Content = _imageStack;
        }

        private void ItemsSourceChanged(BindableObject bindable, IList oldValue, IList newValue)
        {
            if (ItemsSource == null)
            {
                return;
            }

            if (newValue != null && newValue.Count > 0)
            {
                UpdateView(newValue, oldValue);
            }

            var notifyCollection = newValue as INotifyCollectionChanged;
            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged += (sender, args) => { UpdateView(args.NewItems, args.OldItems); };
            }
        }

        private void UpdateView(IList newItems, IList oldItems)
        {
            if (newItems != null)
            {
                foreach (var newItem in newItems)
                {
                    var view = (View) ItemTemplate.CreateContent();
                    if (view is BindableObject bindableObject)
                    {
                        bindableObject.BindingContext = newItem;
                    }

                    view.Margin = new Thickness(0, 0);
                    _imageStack.Children.Add(view);
                }
            }

            if (oldItems != null)
            {
                foreach (var oldItem in oldItems)
                {
                    var item = (ViewerImage) oldItem;
                    var existing = _imageStack.Children.FirstOrDefault(x => x.BindingContext == item);
                    if (existing != null)
                    {
                        _imageStack.Children.Remove(existing);
                    }
                }
            }
        }

        private void UpdateSelectedIndex()
        {
            if (SelectedItem == BindingContext)
            {
                return;
            }

            SelectedIndex = Children
                .Select(c => c.BindingContext)
                .ToList()
                .IndexOf(SelectedItem);
        }

        private void UpdateSelectedItem()
        {
            SelectedItem = SelectedIndex > -1 ? Children[SelectedIndex].BindingContext : null;
        }
    }
}