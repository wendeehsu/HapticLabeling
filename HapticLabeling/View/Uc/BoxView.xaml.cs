using HapticLabeling.Model;
using Windows.UI.Xaml.Controls;

namespace HapticLabeling.View.Uc
{
    public sealed partial class BoxView : UserControl
    {
        public double BoxWidth
        {
            get => rect.Width;
            set => rect.Width = value;
        }

        public double BoxHeight
        {
            get => rect.Height;
            set => rect.Height = value;
        }

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
            BoxWidth = width;
            BoxHeight = height;
        }

        public BoxView()
        {
            InitializeComponent();
        }
    }
}
