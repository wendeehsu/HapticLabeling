using System;
using HapticLabeling.ViewModel;
using Windows.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace HapticLabeling.View
{
    public sealed partial class ConfigPage : Page
    {
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
            ViewModel.ShowLabelDetail = false;
        }

        private void AddHapticLabel_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewModel.ShowLabelDetail = true;
        }
    }
}
