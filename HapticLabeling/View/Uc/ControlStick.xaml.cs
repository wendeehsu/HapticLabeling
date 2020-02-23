using HapticLabeling.Model;
using HapticLabeling.ViewModel.Uc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace HapticLabeling.View
{
    public sealed partial class ControlStick : UserControl
    {
        public ControlStickViewModel ViewModel = new ControlStickViewModel();
        private ObservableCollection<ControllerSelection> _controllers = new ObservableCollection<ControllerSelection>();
        public ObservableCollection<ControllerSelection> Controllers
        {
            get => _controllers;
            set
            {
                _controllers = value;
                ViewModel.UpdateDisplay(value);
            }
        }

        public ControlStick()
        {
            this.InitializeComponent();
        }
    }
}
