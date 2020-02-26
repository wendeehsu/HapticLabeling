using HapticLabeling.Model;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HapticLabeling.View
{
    public sealed partial class HapticLabelMark : UserControl
    {
        public double AccurateStartTime; // save the value from positionSlider

        public double EventStartTime
        {
            get => SpaceTextBlock.Width;
            set => SpaceTextBlock.Width = value;
        }

        public double EventDuration
        {
            get => DurationLine.X2;
            set => DurationLine.X2 = value;
        }

        public HapticLabelMark()
        {
            this.InitializeComponent();
        }

        public void SetDeration(double _eventDuration)
        {
            DurationLine.X2 = _eventDuration - 20;
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
