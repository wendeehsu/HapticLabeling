using HapticLabeling.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
        public List<BoundingBox> Boxes = new List<BoundingBox>();
        public List<HapticEvent> HapticEvents = new List<HapticEvent>();
        public MediaPlayer VideoPlayer = new MediaPlayer();
        public MediaPlayer AudioPlayer = new MediaPlayer();
        public MediaTimelineController MediaTimelineController = null;

        public ObservableCollection<ControllerSelection> _configBoxes = new ObservableCollection<ControllerSelection>();
        public ObservableCollection<ControllerSelection> ConfigBoxes
        {
            get => _configBoxes;
            set => Set(ref _configBoxes, value);
        }


        private double _configViewHeight;
        public double ConfigViewHeight
        {
            get => _configViewHeight;
            set => Set(ref _configViewHeight, value);
        }

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
            set
            {
                Set(ref _showLabelDetail, value);
                if (!value)
                {
                    ResetConfigBoxCheckMode();
                }
            }
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

        private bool _enableConfig = true;
        public bool EnableConfig
        {
            get => _enableConfig;
            set => Set(ref _enableConfig, value);
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

        public void SetConfigViewHeight(JsonBox rangeBox, double width)
        {
            if (rangeBox != null)
            {
                ConfigViewHeight = width * rangeBox.Height / rangeBox.Width;
            }
        }

        public async Task UploadConfig(double width)
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".json");

            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                string text = await FileIO.ReadTextAsync(file);
                var boxes = JsonConvert.DeserializeObject<List<JsonBox>>(text);

                SetConfigViewHeight(boxes[0], width);
                boxes.RemoveAt(0);
                Boxes = GetBoxes(width, boxes);

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
            }
        }

        public List<BoundingBox> GetBoxes(double width, List<JsonBox> jsonBoxes)
        {
            var boundingBoxes = new List<BoundingBox>();
            foreach (var box in jsonBoxes)
            {
                var boundingBox = new BoundingBox(box.X * width / 100, box.Y * ConfigViewHeight / 100);
                boundingBox.Height = box.Height * ConfigViewHeight / 100;
                boundingBox.Width = box.Width * width / 100;
                boundingBox.Name = box.Name;
                boundingBoxes.Add(boundingBox);
            }

            return boundingBoxes;
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

        public int GetInsertIndex(double startTime)
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
                    if(HapticEvents[i].StartTime < startTime)
                    {
                        if(i == 0)
                        {
                            insertIndex = 0;
                            break;
                        }
                        else if (HapticEvents[i-1].StartTime >= startTime)
                        {
                            insertIndex = i;
                            break;
                        }
                    }
                }
            }

            var _event = new HapticEvent(startTime);
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

        public int GetIndex(double starttime)
        {
            var index = -1;
            for (var i = 0; i < HapticEvents.Count; i++)
            {
                if (HapticEvents[i].StartTime == starttime)
                {
                    index = i;
                    break;
                }
            }

            return index;
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
                        Controllers = new ObservableCollection<ControllerSelection>(e.GetActiveProperty());
                        _prevTime = timestamp;
                        return;
                    }
                }
                else
                {
                    break;
                }
            }
            
            if (Math.Abs(timestamp - _prevTime) > tolerence)
            {
                Controllers = null;
            }
        }

        public void RefreshSelectionUI()
        {
            if (Controllers == null || Controllers.Count == 0) return;
            var currentList = Controllers;

            Controllers = new ObservableCollection<ControllerSelection>(currentList);
        }

        public string GetConfigBoxes()
        {
            return JsonConvert.SerializeObject(ConfigBoxes);
        }

        public void ResetConfigBoxCheckMode()
        {
            foreach(var box in ConfigBoxes)
            {
                box.IsChecked = true;
            }
        }
    }
}
