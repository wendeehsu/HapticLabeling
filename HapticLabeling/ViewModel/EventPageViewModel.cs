using HapticLabeling.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class EventPageViewModel : Observable
    {
        public int CurrentIndex = -1;
        public List<Event> Events = new List<Event>();
        public List<HapticEvent> HapticEvents = new List<HapticEvent>();
        public ObservableCollection<ControllerSelection> ConfigBoxes = new ObservableCollection<ControllerSelection>();
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

        private bool _showLabelDetail;
        public bool ShowLabelDetail
        {
            get => _showLabelDetail;
            set => Set(ref _showLabelDetail, value);
        }

        private bool _showAddLabelBtn = true;
        public bool ShowAddLabelBtn
        {
            get => _showAddLabelBtn;
            set => Set(ref _showAddLabelBtn, value);
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

        public ObservableCollection<ControllerSelection> _controllers = new ObservableCollection<ControllerSelection>();
        public ObservableCollection<ControllerSelection> Controllers
        {
            get => _controllers;
            set => Set(ref _controllers, value);
        }

        public void Init()
        {
            MediaTimelineController = new MediaTimelineController();
            VideoPlayer.CommandManager.IsEnabled = false;
            VideoPlayer.TimelineController = MediaTimelineController;

            AudioPlayer.CommandManager.IsEnabled = false;
            AudioPlayer.TimelineController = MediaTimelineController;

            Controllers.Clear();
            // InitControllerSelections();
        }

        public void InitControllerSelections()
        {
            string[] controllers = {
                "Left_Motor",
                "Right_Motor",
                "DPAD_Up",
                "DPAD_Down",
                "DPAD_Left",
                "DPAD_Right",
                "Left_Thumb",
                "Right_Thumb",
                "Left_Shoulder",
                "Right_Shoulder",
                "A",
                "B",
                "X",
                "Y",
                "LeftTrigger",
                "RightTrigger",
                "LeftThumbX",
                "LeftThumbY",
                "RightThumbX",
                "RightThumbY"
            };

            foreach(string i in controllers)
            {
                Controllers.Add(new ControllerSelection() { Name = i });
            }
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
            openPicker.FileTypeFilter.Add(".wmv");

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

        public async Task UploadAction()
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".json");
            openPicker.FileTypeFilter.Add(".txt");
            var file = await openPicker.PickSingleFileAsync();
            if(file != null)
            {
                string text = await FileIO.ReadTextAsync(file);
                Events = JsonConvert.DeserializeObject<List<Event>>(text);
            }
        }

        public async Task UploadConfig()
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".json");

            var file = await openPicker.PickSingleFileAsync();
            if (!(file is null))
            {
                string text = await FileIO.ReadTextAsync(file);
                var boxes = JsonConvert.DeserializeObject<List<JsonBox>>(text);

                // TODO: change "boxes" to listview and grid
                foreach(var box in boxes)
                {
                    if (box.Width > 0)
                    {
                        ConfigBoxes.Add(new ControllerSelection { 
                            Name = box.Name,
                            IsChecked = true
                        });
                    }
                }

                foreach(var config in ConfigBoxes)
                {
                    config.IsChecked = true;
                }
            }
        }

        public void PlayMedia()
        {
            if (VideoPlayer.Source != null || AudioPlayer.Source != null)
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

        public int GetInsertIndex(HapticEvent _event)
        {
            var insertIndex = -1;
            if (HapticEvents == null || HapticEvents.Count == 0)
            {
                insertIndex = 0;
            }
            else
            {
                for (var i = 0; i < HapticEvents.Count; i++)
                {
                    if(HapticEvents[i].StartTime < _event.StartTime)
                    {
                        if(i == 0)
                        {
                            insertIndex = 0;
                            break;
                        }
                        else if (HapticEvents[i-1].StartTime >= _event.StartTime)
                        {
                            insertIndex = i;
                            break;
                        }
                    }
                }
            }

            if(insertIndex == -1)
            {
                HapticEvents.Add(_event);
            }
            else
            {
                HapticEvents.Insert(insertIndex, _event);
            }

            return insertIndex;
        }

        public async Task DownloadLabeledEvent()
        {
            var result = new List<JsonHapticLabel>();
            for (var i = 0; i < HapticEvents.Count; i++)
            {
                result.Add(new JsonHapticLabel(
                    HapticEvents[i].StartTime, 
                    HapticEvents[i].Duration, 
                    HapticEvents[i].Name
                ));
            }
            var json = JsonConvert.SerializeObject(result);
            await SaveJson(json, "hapticLabel");
        }

        public async Task SaveJson(string json, string filename)
        {
            var savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".json" });
            savePicker.SuggestedFileName = DateTime.Now.ToString("HHmmss_MMdd") + "_" + filename;
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                await FileIO.WriteTextAsync(file, json);
                Windows.Storage.Provider.FileUpdateStatus status =
                    await CachedFileManager.CompleteUpdatesAsync(file);
            }
        }


        private double _prevTime = 0;

        public void UpdateSelection(double timestamp)
        {
            double tolerence = 200;

            foreach (var e in Events)
            {
                if (e.TimeStamp <= timestamp)
                {
                    if (Math.Abs(e.TimeStamp - timestamp) <= tolerence)
                    {
                        var newList = e.GetActiveProperty();
                        Controllers = newList;
                        _prevTime = timestamp;
                        return;
                    }
                }
                else
                {
                    break;
                }
            }
            
            if (timestamp - _prevTime > tolerence)
            {
                Controllers.Clear();
            }
        }
    }
}
