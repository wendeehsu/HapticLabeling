using HapticLabeling.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HapticLabeling.ViewModel.Uc
{
    public class ControlStickViewModel : Observable
    {
        #region properties
        private double _leftMotorValue;
        public double LeftMotorValue
        {
            get => _leftMotorValue;
            set => Set(ref _leftMotorValue, value/1000);
        }

        private double _rightMotorValue;
        public double RightMotorValue
        {
            get => _rightMotorValue;
            set => Set(ref _rightMotorValue, value/1000);
        }

        private bool _showDPAD_Up;
        public bool ShowDPAD_Up
        {
            get => _showDPAD_Up;
            set => Set(ref _showDPAD_Up, value);
        }

        private bool _showDPAD_Down;
        public bool ShowDPAD_Down
        {
            get => _showDPAD_Down;
            set => Set(ref _showDPAD_Down, value);
        }

        private bool _showDPAD_Left;
        public bool ShowDPAD_Left
        {
            get => _showDPAD_Left;
            set => Set(ref _showDPAD_Left, value);
        }

        private bool _showDPAD_Right;
        public bool ShowDPAD_Right
        {
            get => _showDPAD_Right;
            set => Set(ref _showDPAD_Right, value);
        }

        private bool _showLeft_Thumb;
        public bool ShowLeft_Thumb
        {
            get => _showLeft_Thumb;
            set => Set(ref _showLeft_Thumb, value);
        }

        private bool _showRight_Thumb;
        public bool ShowRight_Thumb
        {
            get => _showRight_Thumb;
            set => Set(ref _showRight_Thumb, value);
        }

        private bool _showLeft_Shoulder;
        public bool ShowLeft_Shoulder
        {
            get => _showLeft_Shoulder;
            set => Set(ref _showLeft_Shoulder, value);
        }

        private bool _showRight_Shoulder;
        public bool ShowRight_Shoulder
        {
            get => _showRight_Shoulder;
            set => Set(ref _showRight_Shoulder, value);
        }

        private bool _showA;
        public bool ShowA
        {
            get => _showA;
            set => Set(ref _showA, value);
        }

        private bool _showB;
        public bool ShowB
        {
            get => _showB;
            set => Set(ref _showB, value);
        }

        private bool _showX;
        public bool ShowX
        {
            get => _showX;
            set => Set(ref _showX, value);
        }

        private bool _showY;
        public bool ShowY
        {
            get => _showY;
            set => Set(ref _showY, value);
        }

        private bool _showLeftTrigger;
        public bool ShowLeftTrigger
        {
            get => _showLeftTrigger;
            set => Set(ref _showLeftTrigger, value);
        }

        private bool _showRightTrigger;
        public bool ShowRightTrigger
        {
            get => _showRightTrigger;
            set => Set(ref _showRightTrigger, value);
        }

        private bool _showLeftThumbXp;
        public bool ShowLeftThumbXp
        {
            get => _showLeftThumbXp;
            set => Set(ref _showLeftThumbXp, value);
        }

        private bool _showLeftThumbXn;
        public bool ShowLeftThumbXn
        {
            get => _showLeftThumbXn;
            set => Set(ref _showLeftThumbXn, value);
        }

        private bool _showLeftThumbYp;
        public bool ShowLeftThumbYp
        {
            get => _showLeftThumbYp;
            set => Set(ref _showLeftThumbYp, value);
        }

        private bool _showLeftThumbYn;
        public bool ShowLeftThumbYn
        {
            get => _showLeftThumbYn;
            set => Set(ref _showLeftThumbYn, value);
        }

        private bool _showRightThumbXp;
        public bool ShowRightThumbXp
        {
            get => _showRightThumbXp;
            set => Set(ref _showRightThumbXp, value);
        }

        private bool _showRightThumbXn;
        public bool ShowRightThumbXn
        {
            get => _showRightThumbXn;
            set => Set(ref _showRightThumbXn, value);
        }

        private bool _showRightThumbYp;
        public bool ShowRightThumbYp
        {
            get => _showRightThumbYp;
            set => Set(ref _showRightThumbYp, value);
        }

        private bool _showRightThumbYn;
        public bool ShowRightThumbYn
        {
            get => _showRightThumbYn;
            set => Set(ref _showRightThumbYn, value);
        }
        #endregion

        public void UpdateDisplay(ObservableCollection<ControllerSelection> list)
        {
            if (list == null || list.Count == 0) 
            {
                Reset();
                return;
            }
            else
            {
                foreach (var i in list)
                {
                    if (i.Name == "Left_Motor") { LeftMotorValue = i.Value; }
                    if (i.Name == "Right_Motor") { RightMotorValue = i.Value; }
                    if (i.Name == "DPAD_Up") { ShowDPAD_Up = true; }
                    if (i.Name == "DPAD_Down") { ShowDPAD_Down = true; }
                    if (i.Name == "DPAD_Left") { ShowDPAD_Left = true; }
                    if (i.Name == "DPAD_Right") { ShowDPAD_Right = true; }
                    if (i.Name == "Left_Thumb") { ShowLeft_Thumb = true; }
                    if (i.Name == "Right_Thumb") { ShowRight_Thumb = true; }
                    if (i.Name == "Left_Shoulder") { ShowLeft_Shoulder = true; }
                    if (i.Name == "Right_Shoulder") { ShowRight_Shoulder = true; }
                    if (i.Name == "A") { ShowA = true; }
                    if (i.Name == "B") { ShowB = true; }
                    if (i.Name == "X") { ShowX = true; }
                    if (i.Name == "Y") { ShowY = true; }
                    if (i.Name == "LeftTrigger") { ShowLeftTrigger = true; }
                    if (i.Name == "RightTrigger") { ShowRightTrigger = true; }

                    if (i.Name == "LeftThumbX")
                    {
                        ShowLeftThumbXp = i.Value > 0;
                        ShowLeftThumbXn = i.Value < 0;
                    }

                    if (i.Name == "LeftThumbY")
                    {
                        ShowLeftThumbYp = i.Value > 0;
                        ShowLeftThumbYn = i.Value < 0;
                    }

                    if (i.Name == "RightThumbX")
                    {
                        ShowRightThumbXp = i.Value > 0;
                        ShowRightThumbXn = i.Value < 0;
                    }

                    if (i.Name == "RightThumbY")
                    {
                        ShowRightThumbYp = i.Value > 0;
                        ShowRightThumbYn = i.Value < 0;
                    }
                }
            }
        }

        private void Reset()
        {
            LeftMotorValue = 0;
            RightMotorValue = 0;
            ShowDPAD_Up = false;
            ShowDPAD_Down = false;
            ShowDPAD_Left = false;
            ShowDPAD_Right = false;
            ShowLeft_Thumb = false;
            ShowRight_Thumb = false;
            ShowLeft_Shoulder = false;
            ShowRight_Shoulder = false;
            ShowA = false;
            ShowB = false;
            ShowX = false;
            ShowY = false;
            ShowLeftTrigger = false;
            ShowRightTrigger = false;
            ShowLeftThumbXp = false;
            ShowLeftThumbXn = false;
            ShowLeftThumbYp = false;
            ShowLeftThumbYn = false;
            ShowRightThumbXp = false;
            ShowRightThumbXn = false;
            ShowRightThumbYp = false;
            ShowRightThumbYn = false;
        }
    }
}
