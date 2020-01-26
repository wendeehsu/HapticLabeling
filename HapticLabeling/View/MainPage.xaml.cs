using HapticLabeling.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
                StartMedia();
            }
        }

        private async void UploadAction_Click(object sender, RoutedEventArgs e)
        {
            var actionFile = await ViewModel.UploadAction();
            if (actionFile != null)
            {
                ViewModel.SetEvents(actionFile);
                StartMedia();
            }
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ViewModel.SetMediaPosition(e.NewValue);
            StartMedia();
        }

        private void StartMedia()
        {
            if (ViewModel.VideoPlayer.Source != null)
            {
                ViewModel.VideoPlayer.Play();
                videoPlayer.SetMediaPlayer(ViewModel.VideoPlayer);
            }

            if (ViewModel.AudioPlayer.Source != null)
            {
                ViewModel.AudioPlayer.Play();
                audioPlayer.SetMediaPlayer(ViewModel.AudioPlayer);
            }
        }
    }
}
