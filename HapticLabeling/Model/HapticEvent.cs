using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HapticLabeling.Model
{
    public class HapticEvent : Observable
    {
        public double StartTime;
        public double Duration;

        private string _name = "";
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private string _value = "";
        public string Value
        {
            get => _value;
            set => Set(ref _value, value);
        }

        public HapticEvent(double _startTime, double _eventStartTime)
        {
            StartTime = _startTime;
            EventStartTime = _eventStartTime;
        }

        public void SetDuration(double _endTime, double _eventDuration)
        {
            Duration = _endTime - StartTime;
            EventDuration = _eventDuration - 20;
        }

        #region UI Display
        private double _eventStartTime;
        public double EventStartTime
        {
            get => _eventStartTime;
            set => Set(ref _eventStartTime, value);
        }

        private double _eventDuration;
        public double EventDuration
        {
            get => _eventDuration;
            set => Set(ref _eventDuration, value);
        }
        #endregion
    }
}
