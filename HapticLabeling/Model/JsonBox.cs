using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HapticLabeling.Model
{
    public class JsonBox
    {
        public double X;
        public double Y;
        public string Name;
        public double Width;
        public double Height;

        public JsonBox(double x, double y, double w, double h, string name)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
            Name = name;
        }
    }
}
