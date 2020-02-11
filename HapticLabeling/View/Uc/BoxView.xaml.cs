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

        private BoundingBox _box;
        public BoundingBox Box
        {
            get => _box;
            set
            {
                _box = value;
                // https://stackoverflow.com/questions/46579562/uwp-create-dynamic-rectangle
            }
        }

        public BoxView()
        {
            InitializeComponent();
        }
    }
}
