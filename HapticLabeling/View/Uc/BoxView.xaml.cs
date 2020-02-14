using HapticLabeling.Model;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace HapticLabeling.View.Uc
{
    public sealed partial class BoxView : UserControl
    {
        private BoundingBox _boundingBox;
        public BoundingBox BoundingBox
        {
            get => _boundingBox;
            set
            {
                _boundingBox = value;
                if (value != null)
                {
                    x.Width = value.X;
                    y.Height = value.Y;
                }
                // https://stackoverflow.com/questions/46579562/uwp-create-dynamic-rectangle
            }
        }

        public void SetSize(double width, double height)
        {
            if (width < 0 || height < 0) return;
            this.BoundingBox.Width = width;
            this.BoundingBox.Height = height;
            rect.Width = width;
            rect.Height = height;
        }

        public void RemoveHighLight()
        {
            rect.Stroke = new SolidColorBrush(Colors.Red);
        }

        public void HighLight()
        {
            rect.Stroke = new SolidColorBrush(Colors.LightGreen);
        }

        public BoxView()
        {
            InitializeComponent();
        }
    }
}
