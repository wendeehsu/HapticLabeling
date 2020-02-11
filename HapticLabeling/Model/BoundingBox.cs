
namespace HapticLabeling.Model
{
    public class BoundingBox : Observable
    {
        public double leftPos;
        public double topPos;

        private double _x;
        public double X
        {
            get => _x;
            set => Set(ref _x, value);
        }

        private double _y;
        public double Y
        {
            get => _y;
            set => Set(ref _y, value);
        }

        private string _name = "";
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private double _width;
        public double Width
        {
            get => _width;
            set => Set(ref _width, value);
        }

        private double _height;
        public double Height
        {
            get => _height;
            set => Set(ref _height, value);
        }
        
        public BoundingBox(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
