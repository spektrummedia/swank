using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using Xamarin.Forms;
using Switch = Xamarin.Forms.Switch;

namespace Plugin.Swank
{
    public class ViewerImageTemplate : ContentView
    {
        private Viewer Viewer => Parent as Viewer;
        private PanGestureRecognizer _pan = new PanGestureRecognizer();
        public int width => 4096;
        public int height => 2048;

        private readonly Image _image = new Image
        {
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        private readonly Layout<View> _stackLayout = new StackLayout
        {
            HorizontalOptions = LayoutOptions.FillAndExpand,
            Orientation = StackOrientation.Vertical
        };

        private double x, y = 0;
        private int scale = 2;

        public ViewerImageTemplate()
        {
            var currentImage = BindingContext as ViewerImage;
            _image.SetBinding(Image.SourceProperty, nameof(ViewerImage.Source));
            _stackLayout.Children.Add(_image);
            Content = _stackLayout;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var image = BindingContext as ViewerImage;
            if (image != null && image.Is360)
            {
                Create360Controls(image);
            }
        }

        private void Create360Controls(ViewerImage image)
        {
            // Lock-in/lock-out
            var isBlockedSwitch = new Switch();
            isBlockedSwitch.IsToggled = false;
            isBlockedSwitch.Toggled += IsBlockedSwitchOnToggled;

            // Add components to stacklayout
            _stackLayout.Children.Insert(0, isBlockedSwitch);
        }

        private void OnDrag(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                    var absWidth = -Math.Abs(Content.Width - (_image.Width * scale));
                    var minX = Math.Min(300, x + e.TotalX);
                    Content.TranslationX = Math.Max(minX, absWidth);
                    Console.WriteLine($"absHeight: {absWidth} minY: {minX} translationY: {Content.TranslationX}");

                    var absHeight = -Math.Abs(Content.Height - (_image.Height * scale));
                    var minY = Math.Min(300, y + e.TotalY);
                    Content.TranslationY = Math.Max(minY, absHeight);
                    Console.WriteLine($"absHeight: {absHeight} minY: {minY} translationY: {Content.TranslationY}");
                    break;

                case GestureStatus.Completed:
                    // Store the translation applied during the pan
                    x = Content.TranslationX;
                    y = Content.TranslationY;
                    break;
            }
        }

        private IObservable<PanUpdatedEventArgs> PanUpdateAsObservable()
        {
            return Observable.FromEvent<EventHandler<PanUpdatedEventArgs>, PanUpdatedEventArgs>(
                handler => (sender, e) => handler(e),
                handler => _pan.PanUpdated += handler,
                handler => _pan.PanUpdated -= handler
            );
        }

        void SetPanObservable()
        {
            var sampleSeconds = 150; //サンプリングの間隔
            var moveSeconds = 50; //移動アニメーションの時間

            var panObservable = PanUpdateAsObservable();

            var started = panObservable.Where(x => x.StatusType == GestureStatus.Started);
            var completed = panObservable.Where(x => x.StatusType == GestureStatus.Completed);

            started.Subscribe(x => Debug.WriteLine("Pan Started"));
            completed.Subscribe(x => Debug.WriteLine("Pan Completed"));

            var running = panObservable
                .SkipWhile(x => x.StatusType != GestureStatus.Running)  //Running以外はスキップ
                .TakeWhile(x => x.StatusType == GestureStatus.Running); //Runningの間は流し続ける

            //Androidの場合一定時間でイベントをまとめて処理（カクカク対策）
            if (Device.OS == TargetPlatform.Android)
            {
                running = running
                    .Sample(TimeSpan.FromMilliseconds(sampleSeconds))   //一定時間ごとにサンプリング
                    .StartWith(new PanUpdatedEventArgs(GestureStatus.Running, 0, 0, 0)); //最初だけ起点用に発行
            }

            //前の値が必要なので一つずらしたものとZipで合成
            var drag = running.Zip(
                        running.Skip(1),
                        (p, n) => new { PreTotal = new Point(p.TotalX, p.TotalY), Total = new Point(n.TotalX, n.TotalY) }
                    ).Repeat(); //繰り返し


            var dragSub = drag.Subscribe(async p => {

                //移動距離の計算
                var distance = Device.OnPlatform(
                    iOS: new Point(p.Total.X - p.PreTotal.X, p.Total.Y - p.PreTotal.Y),  //iOSは移動いてもTotalはリセットされないので差分を使用する
                    Android: new Point(p.Total.X, p.Total.Y),    // Androidは移動後にTotalがリセットされるのでそのまま使う
                    WinPhone: new Point());

                //現在の位置を取得
                var rectClip = AbsoluteLayout.GetLayoutBounds(_image);

                //座標計算
                var translationX = Math.Max(0, Math.Min(Content.Width - 4000, x + distance.X));
                Console.WriteLine($"translationX: {translationX}");
                Content.TranslationX = translationX;
                var translationY = Math.Max(0, Math.Min(Content.Height - rectClip.Height, y + distance.Y));
                Console.WriteLine($"translationY {translationY}");
                Content.TranslationY = translationY;
                x = translationX;
                y = translationY;
            });
        }

        private void IsBlockedSwitchOnToggled(object sender, ToggledEventArgs toggledEventArgs)
        {
            Viewer.SetIsSwipeEnabled(toggledEventArgs.Value);
            if (toggledEventArgs.Value) 
            {
                // Create gesture
                _stackLayout.GestureRecognizers.Add(_pan);
                SetPanObservable();

                // Image should scale properly
                _image.Aspect = Aspect.Fill;
                _image.Scale = scale;

                // Translation
                //Content.TranslationX = width / 2;
                //Content.TranslationY = height / 2;
            }
            else if (_stackLayout.GestureRecognizers.Any())
            {
                foreach (var gesture in _stackLayout.GestureRecognizers.ToArray())
                {
                    _stackLayout.GestureRecognizers.Remove(gesture);
                }
            }
        }
    }
}