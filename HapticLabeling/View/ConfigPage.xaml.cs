using System;
using HapticLabeling.Model;
using HapticLabeling.View.Uc;
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
        public ConfigPageViewModel ViewModel = new ConfigPageViewModel();

        public ConfigPage()
        {
            this.InitializeComponent();
            ViewModel.Init();
            ViewModel.MediaTimelineController.PositionChanged += MediaTimelineController_PositionChanged;
        }      

        #region Media Control
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
            Frame.Navigate(typeof(InitPage));
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
        #endregion
        
        private void CancelLabel_Click(object sender, RoutedEventArgs e)
        {
            var box = LabelGrid.Children[ViewModel.CurrentIndex] as BoxView;
            box.RemoveHighLight();
            _isAddingEvent = false;
            ViewModel.ShowLabelDetail = false;

            if (!ViewModel.HasRange)
            {
                ViewModel.RangeBox = null;
                LabelGrid.Children.Clear();
            }

            ViewModel.CurrentIndex = -1;
        }

        private void AddHapticLabel_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewModel.SetOriginSize(videoPlayer.ActualHeight, videoPlayer.ActualWidth);
            XTextBlock.Text = "-";
            YTextBlock.Text = "-";
            WidthTextBlock.Text = "-";
            HeightTextBlock.Text = "-";
            NameTextBox.Text = ViewModel.HasRange ? "" : "Range box";
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

            if (ViewModel.CurrentIndex == -1)
            {
                XTextBlock.Text = ViewModel.GetX(x).ToString();
                YTextBlock.Text = ViewModel.GetY(y).ToString();

                // Append Box in ui
                var box = new BoxView();
                box.BoundingBox = new BoundingBox(x, y);

                if (ViewModel.HasRange)
                {
                    var index = ViewModel.GetInsertIndex(box.BoundingBox);
                    if (index == -1)
                    {
                        LabelGrid.Children.Add(box);
                    }
                    else
                    {
                        LabelGrid.Children.Insert(index, box);
                    }

                    ViewModel.CurrentIndex = index;
                }
                else
                {
                    LabelGrid.Children.Add(box);
                    ViewModel.RangeBox = box.BoundingBox;
                    ViewModel.CurrentIndex = 0;
                }
            }
            else
            {
                // Set width, height
                double width = 0;
                double height = 0;
                if (ViewModel.HasRange)
                {
                    width = x - ViewModel.Boxes[ViewModel.CurrentIndex].X;
                    height = y - ViewModel.Boxes[ViewModel.CurrentIndex].Y;
                    ViewModel.SetBoxSize(width, height);
                    var box = LabelGrid.Children[ViewModel.CurrentIndex] as BoxView;
                    box.SetSize(width, height);
                    box.Tapped += Box_Tapped;
                }
                else
                {
                    width = x - ViewModel.RangeBox.X;
                    height = y - ViewModel.RangeBox.Y;
                    ViewModel.RangeBox.Height = height;
                    ViewModel.RangeBox.Width = width;
                }

                WidthTextBlock.Text = ViewModel.GetWidth(width).ToString();
                HeightTextBlock.Text = ViewModel.GetHeight(height).ToString();
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

            if (ViewModel.CurrentIndex == -1)
            {
                XTextBlock.Text = ViewModel.GetX(x).ToString();
                YTextBlock.Text = ViewModel.GetY(y).ToString();
            }
            else
            {
                double width = 0;
                double height = 0;
                if (ViewModel.HasRange)
                {
                    width = x - ViewModel.Boxes[ViewModel.CurrentIndex].X;
                    height = y - ViewModel.Boxes[ViewModel.CurrentIndex].Y;
                }
                else
                {
                    width = x - ViewModel.RangeBox.X;
                    height = y - ViewModel.RangeBox.Y;
                }
                
                WidthTextBlock.Text = ViewModel.GetWidth(width).ToString();
                HeightTextBlock.Text = ViewModel.GetHeight(height).ToString();

                var box = LabelGrid.Children[ViewModel.CurrentIndex] as BoxView;
                box.SetSize(width, height);
            }
        }

        private void DeleteBox_Click(object sender, RoutedEventArgs e)
        {
            var index = ViewModel.CurrentIndex;
            ViewModel.ShowLabelDetail = false;
         
            if (index == -1) return;
            LabelGrid.Children.RemoveAt(index);

            if (ViewModel.HasRange)
            {
                ViewModel.Boxes.RemoveAt(index);
            }
            else
            {
                ViewModel.RangeBox = null;
            }
            
            ViewModel.CurrentIndex = -1;
        }

        private void SaveBox_Click(object sender, RoutedEventArgs e)
        {
            var index = ViewModel.CurrentIndex;
            if (index == -1) return;
            if (ViewModel.HasRange)
            {
                ViewModel.Boxes[index].Name = NameTextBox.Text;

                var box = LabelGrid.Children[index] as BoxView;
                box.BoundingBox = ViewModel.Boxes[index];
                box.RemoveHighLight();

                LabelGrid.Children.RemoveAt(index);
                LabelGrid.Children.Insert(index, box);
            }
            else
            {
                LabelGrid.Children.RemoveAt(index);
                ViewModel.HasRange = true;
            }

            ViewModel.CurrentIndex = -1;
            ViewModel.ShowLabelDetail = false;
        }

        private void Box_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (_isAddingEvent) return;
            var box = sender as BoxView;
            var position = Window.Current.CoreWindow.PointerPosition;
            var x = position.X - Window.Current.Bounds.X - 36;
            var y = position.Y - Window.Current.Bounds.Y;
            if (x < box.BoundingBox.X ||
                y < box.BoundingBox.Y) return;

            RemoveAllHighLights();
            box.HighLight();
            ViewModel.ShowLabelDetail = true;

            var index = ViewModel.Boxes.IndexOf(box.BoundingBox);
            if (index == -1) return;
            ViewModel.CurrentIndex = index;
            
            var boundingBox = box.BoundingBox;
            XTextBlock.Text = ViewModel.GetX(boundingBox.X).ToString();
            YTextBlock.Text = ViewModel.GetY(boundingBox.Y).ToString();
            WidthTextBlock.Text = ViewModel.GetWidth(boundingBox.Width).ToString();
            HeightTextBlock.Text = ViewModel.GetHeight(boundingBox.Height).ToString();
            NameTextBox.Text = boundingBox.Name.ToString();
        }

        private void RemoveAllHighLights()
        {
            for (int i = 0; i < LabelGrid.Children.Count; i++)
            {
                var box = LabelGrid.Children[i] as BoxView;
                box.RemoveHighLight();
            }
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.DownloadConfigBox();
        }
    }
}
