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
            var videoFile = await ViewModel.UploadVideo();
            if (!(videoFile is null))
            {
                var _mediaSource = MediaSource.CreateFromStorageFile(videoFile);
                var _mediaPlayer = new MediaPlayer();
                _mediaPlayer.Source = _mediaSource;
                _mediaPlayer.Play();
                media.SetMediaPlayer(_mediaPlayer);
            }
        }

        private async void UploadAudio_Click(object sender, RoutedEventArgs e)
        {
            var audioFile = await ViewModel.UploadAudio();
            if (!(audioFile is null))
            {
                var _mediaSource = MediaSource.CreateFromStorageFile(audioFile);
                var _mediaPlayer = new MediaPlayer();
                _mediaPlayer.Source = _mediaSource;
                _mediaPlayer.Play();
                media.SetMediaPlayer(_mediaPlayer);
            }
        }

        private async void UploadAction_Click(object sender, RoutedEventArgs e)
        {
            var actionFile = await ViewModel.UploadAction();
            if (!(actionFile is null))
            {
                // TODO: decode action
            }
        }
    }
}
