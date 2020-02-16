using HapticLabeling.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;

namespace HapticLabeling.ViewModel
{
    public class ConfigPageViewModel : Observable
    {
        public MediaPlayer VideoPlayer = new MediaPlayer();
        public MediaPlayer AudioPlayer = new MediaPlayer();
        public MediaTimelineController MediaTimelineController = null;
        public int CurrentIndex = -1;
        public List<BoundingBox> Boxes = new List<BoundingBox>();
        public BoundingBox RangeBox;
        public double OriginHeight = 0;
        public double OriginWidth = 0;

        private double _mediaLength;
        public double MediaLength
        {
            get => _mediaLength;
            set => Set(ref _mediaLength, value);
        }

        private bool _hasRange;
        public bool HasRange
        {
            get => _hasRange;
            set => Set(ref _hasRange, value);
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

        public void SetOriginSize(double h, double w)
        {
            OriginHeight = h;
            OriginWidth = w;
        }

        public void Init()
        {
            MediaTimelineController = new MediaTimelineController();
            VideoPlayer.CommandManager.IsEnabled = false;
            VideoPlayer.TimelineController = MediaTimelineController;

            AudioPlayer.CommandManager.IsEnabled = false;
            AudioPlayer.TimelineController = MediaTimelineController;
        }

        #region Media Player
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
        #endregion

        public int GetInsertIndex(BoundingBox _box)
        {
            var insertIndex = -1;
            if (Boxes != null && Boxes.Count != 0)
            {
                for (var i = 0; i < Boxes.Count; i++)
                {
                    // larger x should be placed at the lower level
                    if (Boxes[i].X < _box.X)
                    {
                        if (i == 0)
                        {
                            insertIndex = 0;
                            break;
                        }
                        else if (Boxes[i - 1].X >= _box.X)
                        {
                            insertIndex = i;
                            break;
                        }
                    }
                }
            }

            if (insertIndex == -1)
            {
                Boxes.Add(_box);
            }
            else
            {
                Boxes.Insert(insertIndex, _box);
            }

            return insertIndex;
        }

        public void SetBoxSize(double width, double height)
        {
            if (CurrentIndex == -1) return;
            Boxes[CurrentIndex].Width = width;
            Boxes[CurrentIndex].Height = height;
        }

        public async Task DownloadConfigBox()
        {
            var result = new List<JsonBox>();
            for(var i = 0; i < Boxes.Count; i ++)
            {
                result.Add(new JsonBox
                (
                    GetX(Boxes[i].X), 
                    GetY(Boxes[i].Y), 
                    GetWidth(Boxes[i].Width), 
                    GetHeight(Boxes[i].Height), 
                    Boxes[i].Name
                ));
            }
            var json = JsonConvert.SerializeObject(result);
            var savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".json" });
            savePicker.SuggestedFileName = DateTime.Now.ToString("HHmmss_MMdd") + "_config";
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                await FileIO.WriteTextAsync(file, json);
                Windows.Storage.Provider.FileUpdateStatus status =
                    await CachedFileManager.CompleteUpdatesAsync(file);
            }
        }

        public double GetX(double x)
        {
            if (!HasRange) return x;
            return Math.Round((x - RangeBox.X) / RangeBox.Width, 4) * 100;
        }

        public double GetY(double y)
        {
            if (!HasRange) return y;
            return Math.Round((y - RangeBox.Y) / RangeBox.Height, 4) * 100;
        }

        public double GetWidth(double w)
        {
            if (!HasRange) return w;
            return Math.Round(w/RangeBox.Width, 4) * 100;
        }

        public double GetHeight(double h)
        {
            if (!HasRange) return h;
            return Math.Round(h / RangeBox.Height, 4) * 100;
        }
    }
}
