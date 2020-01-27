using HapticLabeling.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HapticLabeling.ViewModel
{
    public class HapticLabelMarkViewModel : Observable
    {
        private HapticEvent _event;
        public HapticEvent Event
        {
            get => _event;
            set => Set(ref _event, value);
        }
    }
}
