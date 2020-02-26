using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HapticLabeling.Model
{
    public class JsonHapticLabel
    {
        public double StartTime;
        public double Duration;
        public string Name;
        public List<JsonBox> ConfigBoxes;
        public List<Event> ControllerEvents;

        public JsonHapticLabel(double _startTime, double _duration, string _name)
        {
            StartTime = _startTime;
            Duration = _duration;
            Name = _name;
        }
    }
}
