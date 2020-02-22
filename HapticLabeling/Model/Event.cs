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
            var list = new ObservableCollection<ControllerSelection>();

            if (Left_Motor > 0) { 
                list.Add(new ControllerSelection() 
                { 
                    Name = "Left_Motor", 
                    IsChecked = true 
                }); 
            };
            if (Right_Motor > 0)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "Right_Motor",
                    IsChecked = true
                });
            };
            if (DPAD_Up)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "DPAD_Up",
                    IsChecked = true
                });
            };
            if (DPAD_Down)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "DPAD_Down",
                    IsChecked = true
                });
            };
            if (DPAD_Left)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "DPAD_Left",
                    IsChecked = true
                });
            };
            if (DPAD_Right)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "DPAD_Right",
                    IsChecked = true
                });
            };
            if (Left_Thumb)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "Left_Thumb",
                    IsChecked = true
                });
            };
            if (Right_Thumb)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "Right_Thumb",
                    IsChecked = true
                });
            };
            if (Left_Shoulder)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "Left_Shoulder",
                    IsChecked = true
                });
            };
            if (Right_Shoulder)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "Right_Shoulder",
                    IsChecked = true
                });
            };
            if (A)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "A",
                    IsChecked = true
                });
            };
            if (B)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "B",
                    IsChecked = true
                });
            };
            if (X)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "X",
                    IsChecked = true
                });
            };
            if (Y)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "Y",
                    IsChecked = true
                });
            };
            if (LeftTrigger > 0)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "LeftTrigger",
                    IsChecked = true
                });
            };
            if (RightTrigger > 0)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "RightTrigger",
                    IsChecked = true
                });
            };
            if (LeftThumbX != 0)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "LeftThumbX",
                    IsChecked = true
                });
            };
            if (LeftThumbY != 0)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "LeftThumbY",
                    IsChecked = true
                });
            };
            if (RightThumbX != 0)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "RightThumbX",
                    IsChecked = true
                });
            };
            if (RightThumbY != 0)
            {
                list.Add(new ControllerSelection()
                {
                    Name = "RightThumbY",
                    IsChecked = true
                });
            };

            return list;
        }
    }
}
