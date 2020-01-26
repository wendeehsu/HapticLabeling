using HapticLabeling.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;

namespace HapticLabeling.ViewModel
{
    public class MainPageViewModel : Observable
    {
        public List<Event> Events = new List<Event>();
        public MediaPlayer VideoPlayer = new MediaPlayer();
        public MediaPlayer AudioPlayer = new MediaPlayer();
        public MediaTimelineController MediaTimelineController = null;

        private double _mediaLength;
        public double MediaLength
        {
            get => _mediaLength;
            set => Set(ref _mediaLength, value);
        }

        private double _mediaPosition;
        public double MediaPosition
        {
            get => _mediaPosition;
            set => Set(ref _mediaPosition, value);
        }

        private bool _showPauseBtn;
        public bool ShowPauseBtn
        {
            get => _showPauseBtn;
            set => Set(ref _showPauseBtn, value);
        }

        private bool _enableVideo = true;
        public bool EnableVideo
        {
            get => _enableVideo;
            set => Set(ref _enableVideo, value);
        }

        private bool _enableAudio = true;
        public bool EnableAudio
        {
            get => _enableAudio;
            set
            {
                Set(ref _enableAudio, value);
                AudioPlayer.IsMuted = !value;
            }
        }

        private bool _enableEvents = true;
        public bool EnableEvents
        {
            get => _enableEvents;
            set => Set(ref _enableEvents, value);
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
            if (!(file is null))
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
            if (!(file is null))
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

        public async Task<StorageFile> UploadAction()
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".json");
            openPicker.FileTypeFilter.Add(".txt");
            var file = await openPicker.PickSingleFileAsync();
            return file;
        }

        public async void SetEvents(StorageFile file)
        {
            string text = await FileIO.ReadTextAsync(file);
            Events = JsonConvert.DeserializeObject<List<Event>>(text);

            // TODO: set events.
            Debug.WriteLine(Events.Count);
        }

        public void PlayMedia()
        {
            if (VideoPlayer.Source != null && AudioPlayer.Source != null)
            {
                if(VideoPlayer.PlaybackSession.Position == TimeSpan.Zero)
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
