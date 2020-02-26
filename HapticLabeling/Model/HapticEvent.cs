using Newtonsoft.Json;
using System.Collections.Generic;

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

        public HapticEvent(double _startTime)
        {
            StartTime = _startTime;
        }

        private string _relatedConfigs;
        public string RelatedConfigs
        {
            get => _relatedConfigs;
            set => Set(ref _relatedConfigs, value);
        }

        public void SetRelatedConfigs(string json)
        {
            RelatedConfigs = json;
        }

        public List<ControllerSelection> GetConfigBoxes()
        {
            return JsonConvert.DeserializeObject<List<ControllerSelection>>(RelatedConfigs);
        }
    }
}
