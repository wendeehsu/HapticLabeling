using HapticLabeling.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;

namespace HapticLabeling.ViewModel
{
    public class ConfigPageViewModel : Observable
    {
        public MediaPlayer VideoPlayer = new MediaPlayer();
        public MediaPlayer AudioPlayer = new MediaPlayer();
        public MediaTimelineController MediaTimelineController = null;

        private double _mediaLength;
        public double MediaLength
        {
            get => _mediaLength;
            set => Set(ref _mediaLength, value);
        }

        private bool _showPauseBtn;
        public bool ShowPauseBtn
        {
            get => _showPauseBtn;
            set => Set(ref _showPauseBtn, value);
        }

        private bool _showLabelDetail;
        public bool ShowLabelDetail
        {
            get => _showLabelDetail;
            set => Set(ref _showLabelDetail, value);
        }

        public void Init()
        {
            MediaTimelineController = new MediaTimelineController();
            VideoPlayer.CommandManager.IsEnabled = false;
            VideoPlayer.TimelineController = MediaTimelineController;

            AudioPlayer.CommandManager.IsEnabled = false;
            AudioPlayer.TimelineController = MediaTimelineController;
        }

        public async Task UploadVideo()
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".mov");
            openPicker.FileTypeFilter.Add(".wmv");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var _mediaSource = MediaSource.CreateFromStorageFile(file);
                Windows.Storage.FileProperties.VideoProperties videoProperties = await file.Properties.GetVideoPropertiesAsync();
                Duration videoDuration = videoProperties.Duration;
                var length = videoDuration.TimeSpan.TotalMilliseconds;
                if (length > MediaLength)
                {
                    MediaLength = length;
                }
                VideoPlayer.Source = _mediaSource;
            }
        }

        public async Task UploadAudio()
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".mp3");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var _mediaSource = MediaSource.CreateFromStorageFile(file);
                Windows.Storage.FileProperties.VideoProperties audioProperties = await file.Properties.GetVideoPropertiesAsync();
                Duration audioDuration = audioProperties.Duration;
                var length = audioDuration.TimeSpan.TotalMilliseconds;
                if (length > MediaLength)
                {
                    MediaLength = length;
                }
                AudioPlayer.Source = _mediaSource;
            }
        }

        public void PlayMedia()
        {
            if (VideoPlayer.Source != null || AudioPlayer.Source != null)
            {
                if (VideoPlayer.PlaybackSession.Position == TimeSpan.Zero)
                {
                    MediaTimelineController.Start();
                }
                else
                {
                    MediaTimelineController.Resume();
                }
            }
        }

        public void PauseMedia()
        {
            MediaTimelineController.Pause();
        }


    }
}
