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
            _isAddingEvent = false;
            ViewModel.ShowLabelDetail = false;
            ViewModel.CurrentIndex = -1;
        }

        private void AddHapticLabel_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
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
                XTextBlock.Text = x.ToString();
                YTextBlock.Text = y.ToString();

                // Append Box in ui
                var box = new BoxView();
                box.BoundingBox = new BoundingBox(x, y);

                var index = ViewModel.GetInsertIndex(box.BoundingBox);
                if (index == -1)
                {
                    LabelGrid.Children.Add(box);
                }
                else
                {
                    LabelGrid.Children.Insert(index, box);
                }

                ViewModel.CurrentIndex = LabelGrid.Children.IndexOf(box);
            }
            else
            {
                // Set width, height
                var width = x - ViewModel.Boxes[ViewModel.CurrentIndex].X;
                var height = y - ViewModel.Boxes[ViewModel.CurrentIndex].Y;
                WidthTextBlock.Text = width.ToString();
                HeightTextBlock.Text = height.ToString();

                ViewModel.SetBoxSize(width, height);
                var box = LabelGrid.Children[ViewModel.CurrentIndex] as BoxView;
                box.SetSize(width, height);
                
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
                XTextBlock.Text = x.ToString();
                YTextBlock.Text = y.ToString();
            }
            else
            {
                var width = x - ViewModel.Boxes[ViewModel.CurrentIndex].X;
                var height = y - ViewModel.Boxes[ViewModel.CurrentIndex].Y;
                WidthTextBlock.Text = width.ToString();
                HeightTextBlock.Text = height.ToString();

                var box = LabelGrid.Children[ViewModel.CurrentIndex] as BoxView;
                box.SetSize(width, height);
            }
        }

        private void DeleteBox_Click(object sender, RoutedEventArgs e)
        {
            var index = ViewModel.CurrentIndex;
            if (index == -1) return;
            LabelGrid.Children.RemoveAt(index);
            ViewModel.Boxes.RemoveAt(index);
            ViewModel.ShowLabelDetail = false;
            ViewModel.CurrentIndex = -1;
        }

        private void SaveBox_Click(object sender, RoutedEventArgs e)
        {
            var index = ViewModel.CurrentIndex;
            if (index == -1) return;
            ViewModel.Boxes[index].Name = NameTextBox.Text;

            var box = LabelGrid.Children[index] as BoxView;
            box.BoundingBox = ViewModel.Boxes[index];

//            label.RemoveHighlight();

            LabelGrid.Children.RemoveAt(index);
            LabelGrid.Children.Insert(index, box);

            ViewModel.CurrentIndex = -1;
            ViewModel.ShowLabelDetail = false;
        }
    }
}
