using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HapticLabeling.Model
{
    public class Event
    {
        public double TimeStamp;
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

        public ObservableCollection<ControllerSelection> GetActiveProperty()
        {
            var list = new List<ControllerSelection>();

            list.Add(new ControllerSelection()
            {
                Name = "Left_Motor",
                IsChecked = Left_Motor > 0,
                Value = Left_Motor
            });
            list.Add(new ControllerSelection()
            {
                Name = "Right_Motor",
                IsChecked = Right_Motor > 0,
                Value = Right_Motor
            });
            list.Add(new ControllerSelection()
            {
                Name = "DPAD_Up",
                IsChecked = DPAD_Up
            });
            list.Add(new ControllerSelection()
            {
                Name = "DPAD_Down",
                IsChecked = DPAD_Down
            });
            list.Add(new ControllerSelection()
            {
                Name = "DPAD_Left",
                IsChecked = DPAD_Left
            });
            list.Add(new ControllerSelection()
            {
                Name = "DPAD_Right",
                IsChecked = DPAD_Right
            });
            list.Add(new ControllerSelection()
            {
                Name = "Left_Thumb",
                IsChecked = Left_Thumb
            });
            list.Add(new ControllerSelection()
            {
                Name = "Right_Thumb",
                IsChecked = Right_Thumb
            });
            list.Add(new ControllerSelection()
            {
                Name = "Left_Shoulder",
                IsChecked = Left_Shoulder
            });
            list.Add(new ControllerSelection()
            {
                Name = "Right_Shoulder",
                IsChecked = Right_Shoulder
            });
            list.Add(new ControllerSelection()
            {
                Name = "A",
                IsChecked = A
            });
            list.Add(new ControllerSelection()
            {
                Name = "B",
                IsChecked = B
            });
            list.Add(new ControllerSelection()
            {
                Name = "X",
                IsChecked = X
            });
            list.Add(new ControllerSelection()
            {
                Name = "Y",
                IsChecked = Y
            });
            list.Add(new ControllerSelection()
            {
                Name = "LeftTrigger",
                IsChecked = LeftTrigger > 0
            });
            list.Add(new ControllerSelection()
            {
                Name = "RightTrigger",
                IsChecked = RightTrigger > 0
            });
            list.Add(new ControllerSelection()
            {
                Name = "LeftThumbX",
                IsChecked = LeftThumbX != 0,
                Value = LeftThumbX
            });
            list.Add(new ControllerSelection()
            {
                Name = "LeftThumbY",
                IsChecked = LeftThumbY != 0,
                Value = LeftThumbY
            });
            list.Add(new ControllerSelection()
            {
                Name = "RightThumbX",
                IsChecked = RightThumbX != 0,
                Value = RightThumbX
            });
            list.Add(new ControllerSelection()
            {
                Name = "RightThumbY",
                IsChecked = RightThumbY != 0,
                Value = RightThumbY
            });

            return new ObservableCollection<ControllerSelection>(list.OrderByDescending(i => i.IsChecked));
        }

        public void UpdateValue(ControllerSelection cs, Event e)
        {
            switch (cs.Name)
            {
                case "Left_Motor":
                    Left_Motor = cs.IsChecked ? e.Left_Motor : 0;
                    break;
                case "Right_Motor":
                    Right_Motor = cs.IsChecked ? e.Right_Motor : 0;
                    break;
                case "DPAD_Up":
                    DPAD_Up = cs.IsChecked;
                    break;
                case "DPAD_Down":
                    DPAD_Down = cs.IsChecked;
                    break;
                case "DPAD_Left":
                    DPAD_Left = cs.IsChecked;
                    break;
                case "DPAD_Right":
                    DPAD_Right = cs.IsChecked;
                    break;
                case "Left_Thumb":
                    Left_Thumb = cs.IsChecked;
                    break;
                case "Right_Thumb":
                    Right_Thumb = cs.IsChecked;
                    break;
                case "Left_Shoulder":
                    Left_Shoulder = cs.IsChecked;
                    break;
                case "Right_Shoulder":
                    Right_Shoulder = cs.IsChecked;
                    break;
                case "A":
                    A = cs.IsChecked;
                    break;
                case "B":
                    B = cs.IsChecked;
                    break;
                case "X":
                    X = cs.IsChecked;
                    break;
                case "Y":
                    Y = cs.IsChecked;
                    break;
                case "LeftTrigger":
                    LeftTrigger = cs.IsChecked ? e.LeftTrigger : 0;
                    break;
                case "RightTrigger":
                    RightTrigger = cs.IsChecked ? e.RightTrigger : 0;
                    break;
                case "LeftThumbX":
                    LeftThumbX = cs.IsChecked ? e.LeftThumbX : 0;
                    break;
                case "LeftThumbY":
                    LeftThumbY = cs.IsChecked ? e.LeftThumbY : 0;
                    break;
                case "RightThumbX":
                    RightThumbX = cs.IsChecked ? e.RightThumbX : 0;
                    break;
                case "RightThumbY":
                    LeftThumbY = cs.IsChecked ? e.RightThumbY : 0;
                    break;
            };
        }
    }
}
