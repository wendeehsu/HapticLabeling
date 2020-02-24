﻿using HapticLabeling.Model;
using HapticLabeling.View;
using HapticLabeling.View.Uc;
using HapticLabeling.ViewModel;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace HapticLabeling.View
{
    public sealed partial class EventPage : Page
    {
        public EventPageViewModel ViewModel = new EventPageViewModel();
        public EventPage()
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
            try
            {
                await ViewModel.UploadAction();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ViewModel.UpdateSelection(e.NewValue);
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
            label.Event = new HapticEvent(PositionSlider.Value, PositionSlider.ActualWidth * PositionSlider.Value / ViewModel.MediaLength);
            
            var index = ViewModel.GetInsertIndex(label.Event);
            if(index == -1)
            {
                LabelGrid.Children.Add(label);
            }
            else
            {
                LabelGrid.Children.Insert(index, label);
            }

            ViewModel.CurrentIndex = LabelGrid.Children.IndexOf(label);
        }

        private void Label_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var label = sender as HapticLabelMark;
            var position = Window.Current.CoreWindow.PointerPosition;
            var x = position.X - Window.Current.Bounds.X - 70;
            if (x <= label.Event.EventStartTime) return;

            RemoveAllHighLights();
            label.HighLight();
            ViewModel.ShowLabelDetail = true;

            var index = ViewModel.HapticEvents.IndexOf(label.Event);
            if (index == -1) return;
            ViewModel.CurrentIndex = index;
            StartTimeTextBlock.Text = ViewModel.HapticEvents[index].StartTime.ToString();
            DurationTextBlock.Text = ViewModel.HapticEvents[index].Duration.ToString();
            NameTextBox.Text = ViewModel.HapticEvents[index].Name.ToString();
        }

        private void SetLabelDuration()
        {
            var index = ViewModel.CurrentIndex;
            if (index == -1) return;
            var label = LabelGrid.Children[index] as HapticLabelMark;
            var duration = PositionSlider.Value - label.Event.StartTime;
            LabelGrid.Children.RemoveAt(index);
            if (duration >= 0)
            {
                var length = PositionSlider.ActualWidth * duration / ViewModel.MediaLength;
                label.SetDeration(PositionSlider.Value, length);
                label.Tapped += Label_Tapped;
                LabelGrid.Children.Insert(index, label);
            }
        }

        private void AddHapticLabel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.ShowAddLabelBtn = false;
            AddInitLabel();
        }

        private void EndHapticLabel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.ShowAddLabelBtn = true;
            SetLabelDuration();
        }

        private void DeleteLabel_Click(object sender, RoutedEventArgs e)
        {
            var index = ViewModel.CurrentIndex;
            if (index == -1) return;
            LabelGrid.Children.RemoveAt(index);
            ViewModel.HapticEvents.RemoveAt(index);
            ViewModel.ShowLabelDetail = false;
        }

        private void SaveLabel_Click(object sender, RoutedEventArgs e)
        {
            var index = ViewModel.CurrentIndex;
            if (index == -1) return;
            ViewModel.HapticEvents[index].Name = NameTextBox.Text;

            var label = LabelGrid.Children[index] as HapticLabelMark;
            label.Event = ViewModel.HapticEvents[index];
            label.RemoveHighlight();
            LabelGrid.Children.RemoveAt(index);
            LabelGrid.Children.Insert(index, label);

            ViewModel.ShowLabelDetail = false;
        }

        private void CancelLabel_Click(object sender, RoutedEventArgs e)
        {
            var label = LabelGrid.Children[ViewModel.CurrentIndex] as HapticLabelMark;
            label.RemoveHighlight();
            ViewModel.ShowLabelDetail = false;
        }

        private void RemoveAllHighLights()
        {
            for(int i = 0; i < LabelGrid.Children.Count; i ++)
            {
                var label = LabelGrid.Children[i] as HapticLabelMark;
                label.RemoveHighlight();
            }
        }

        private void BackBtn_Clicked(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(InitPage));
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 
            // 1. new controller.json
            // 2. config file with respect to time
            // 3. label files
            await ViewModel.DownloadLabeledEvent();
        }

        private async void UploadConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ViewModel.UploadConfig(BoxGridView.ActualWidth);
                foreach (var box in ViewModel.Boxes)
                {
                    var boxView = new BoxView();
                    boxView.BoundingBox = box;
                    boxView.SetSize(box.Width, box.Height);
                    BoxGridView.Children.Add(boxView);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void ControllerSelection_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox &&
                checkBox.DataContext is ControllerSelection cs)
            {
                var controller = ViewModel.Controllers.First(c => c.Name == cs.Name);
                if (controller != null && controller.IsChecked != true)
                {
                    cs.IsChecked = true;
                    ViewModel.RefreshSelectionUI();
                }
            }
        }

        private void ControllerSelection_UnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox &&
                checkBox.DataContext is ControllerSelection cs)
            {
                var controller = ViewModel.Controllers.First(c => c.Name == cs.Name);
                if (controller != null && controller.IsChecked != false)
                {
                    cs.IsChecked = false;
                    ViewModel.RefreshSelectionUI();
                }
            }
        }

        private void ConfigBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox &&
               checkBox.DataContext is ControllerSelection cs)
            {
                var controller = ViewModel.ConfigBoxes.First(c => c.Name == cs.Name);
                if (controller != null && controller.IsChecked != true)
                {
                    cs.IsChecked = true;
                    foreach (var childBox in BoxGridView.Children)
                    {
                        if (childBox is BoxView childBoxView &&
                            childBoxView.BoundingBox.Name == cs.Name)
                        {
                            childBoxView.Visibility = Visibility.Visible;
                            return;
                        }
                    }
                }
            }
        }

        private void ConfigBox_UnChecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox &&
               checkBox.DataContext is ControllerSelection cs)
            {
                var controller = ViewModel.ConfigBoxes.First(c => c.Name == cs.Name);
                if (controller != null && controller.IsChecked != false)
                {
                    cs.IsChecked = false;
                    foreach (var childBox in BoxGridView.Children)
                    {
                        if (childBox is BoxView childBoxView &&
                            childBoxView.BoundingBox.Name == cs.Name)
                        {
                            childBoxView.Visibility = Visibility.Collapsed;
                            return;
                        }
                    }
                }
            }
        }
    }
}
