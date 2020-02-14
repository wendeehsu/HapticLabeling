using HapticLabeling.Helper;
using HapticLabeling.Model;
using HapticLabeling.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HapticLabeling.View
{
    public sealed partial class HapticLabelMark : UserControl
    {
        public double EventStartTime;
        public double EventDuration;

        public HapticLabelMark()
        {
            this.InitializeComponent();
        }

        private HapticEvent _event;
        public HapticEvent Event
        {
            get => _event;
            set
            {
                _event = value;
                SpaceTextBlock.Width = _event.EventStartTime;
                DurationLine.X2 = _event.EventDuration;
            }
        }

        public void SetDeration(double _endTime, double _eventDuration)
        {
            Event.Duration = _endTime - Event.StartTime;
            Event.EventDuration = _eventDuration - 20;
            DurationLine.X2 = Event.EventDuration;
        }

        public void HighLight()
        {
            Ellipse.Stroke = new SolidColorBrush(Colors.Red);
            DurationLine.Stroke = new SolidColorBrush(Colors.Red);
        }

        public void RemoveHighlight()
        {
            Ellipse.Stroke = new SolidColorBrush(Colors.MediumPurple);
            DurationLine.Stroke = new SolidColorBrush(Colors.LightYellow);
        }
    }
}
