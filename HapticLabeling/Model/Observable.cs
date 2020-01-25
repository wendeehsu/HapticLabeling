using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HapticLabeling.Model
{
    public class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<T>(ref T field, T newValue = default(T), [CallerMemberName]string propertyName = null)
        {
            if (Equals(field, newValue))
            {
                return false;
            }

            field = newValue;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        protected void NotifyPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void NotifyAllPropertiesChange()
        {
            NotifyPropertyChanged("");
        }
    }
}
