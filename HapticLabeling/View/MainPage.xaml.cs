using HapticLabeling.Model;
using HapticLabeling.View;
using HapticLabeling.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HapticLabeling
{
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel = new MainPageViewModel();
        public MainPage()
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

        private async void UploadAction_Click(object sender, RoutedEventArgs e)
        {
            var actionFile = await ViewModel.UploadAction();
            if (actionFile != null)
            {
                ViewModel.SetEvents(actionFile);
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

        private void AddInitLabel()
        {
            if (ViewModel.VideoPlayer.PlaybackSession.Position <= TimeSpan.FromMilliseconds(50)) return;
            var label = new HapticLabelMark();
            label.ViewModel.Event = new HapticEvent(PositionSlider.Value, ViewModel.MediaLength, PositionSlider.ActualWidth);
            LabelGrid.Children.Insert(0, label);
        }

        private void AddHapticLabel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.ShowAddLabelBtn = false;
            AddInitLabel();
        }

        private void EndHapticLabel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.ShowAddLabelBtn = true;
        }
    }
}
