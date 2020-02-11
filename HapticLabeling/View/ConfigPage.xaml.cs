using System;
using HapticLabeling.Model;
using HapticLabeling.ViewModel;
using Windows.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace HapticLabeling.View
{
    public sealed partial class ConfigPage : Page
    {
        private bool _isAddingEvent = false;
        private bool _isScaling = false;
        public ConfigPageViewModel ViewModel = new ConfigPageViewModel();

        public ConfigPage()
        {
            this.InitializeComponent();
            ViewModel.Init();
            ViewModel.MediaTimelineController.PositionChanged += MediaTimelineController_PositionChanged;
        }

        private async void MediaTimelineController_PositionChanged(MediaTimelineController sender, object args)
        {
            if (ViewModel.MediaLength != 0)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    PositionSlider.Value = sender.Position.TotalMilliseconds;
                });
            }
        }

        private void BackBtn_Clicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(InitPage));
        }

        private async void UploadVideo_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.UploadVideo();
            if (ViewModel.VideoPlayer.Source != null)
            {
                videoPlayer.SetMediaPlayer(ViewModel.VideoPlayer);
            }
        }

        private async void UploadAudio_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.UploadAudio();
            if (ViewModel.AudioPlayer.Source != null)
            {
                audioPlayer.SetMediaPlayer(ViewModel.AudioPlayer);
            }
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ViewModel.MediaTimelineController.Position = TimeSpan.FromMilliseconds(e.NewValue);
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowPauseBtn = true;
            ViewModel.PlayMedia();
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowPauseBtn = false;
            ViewModel.PauseMedia();
        }

        private void CancelLabel_Click(object sender, RoutedEventArgs e)
        {
            _isAddingEvent = false;
            _isScaling = false;
            ViewModel.ShowLabelDetail = false;
            ViewModel.SelectedBox = null;
        }

        private void AddHapticLabel_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewModel.SelectedBox = new BoundingBox(0,0);
            ViewModel.ShowLabelDetail = true;
            _isAddingEvent = true;
        }

        private void LabelGrid_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!_isAddingEvent) return;
            var position = Window.Current.CoreWindow.PointerPosition;
            var x = position.X - Window.Current.Bounds.X - 36;
            var y = position.Y - Window.Current.Bounds.Y;
            x = Math.Round(x, 2);
            y = Math.Round(y, 2);

            if (!_isScaling)
            {
                ViewModel.SelectedBox.X = x;
                ViewModel.SelectedBox.Y = y;
                _isScaling = true;
            }
            else
            {
                // Set width, height
                ViewModel.SelectedBox.Width = x - ViewModel.SelectedBox.X;
                ViewModel.SelectedBox.Height = y - ViewModel.SelectedBox.Y;
                _isScaling = false;
                _isAddingEvent = false;
            }
        }

        private void LabelGrid_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!_isAddingEvent) return;
            var position = Window.Current.CoreWindow.PointerPosition;
            var x = position.X - Window.Current.Bounds.X - 36;
            var y = position.Y - Window.Current.Bounds.Y;
            x = Math.Round(x, 2);
            y = Math.Round(y, 2);

            if (!_isScaling)
            {
                ViewModel.SelectedBox.X = x;
                ViewModel.SelectedBox.Y = y;
            }
            else
            {
                ViewModel.SelectedBox.Width = x - ViewModel.SelectedBox.X;
                ViewModel.SelectedBox.Height = y - ViewModel.SelectedBox.Y;
            }
        }
    }
}
