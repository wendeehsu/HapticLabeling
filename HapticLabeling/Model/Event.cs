using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HapticLabeling.Model
{
    public class Event
    {
        public int TimeStamp;
        public int Left_Motor;
        public int Right_Motor;
        public bool DPAD_Up;
        public bool DPAD_Down;
        public bool DPAD_Left;
        public bool DPAD_Right;
        public bool Start;
        public bool Back;
        public bool Left_Thumb;
        public bool Right_Thumb;
        public bool Left_Shoulder;
        public bool Right_Shoulder;
        public bool A;
        public bool B;
        public bool X;
        public bool Y;
        public int LeftTrigger;
        public int RightTrigger;
        public int LeftThumbX;
        public int LeftThumbY;
        public int RightThumbX;
        public int RightThumbY;
    }
}
