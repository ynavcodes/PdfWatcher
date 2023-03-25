using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PdfWatcher
{
    public class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool SetValue<T>(ref T currentValue, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(currentValue, newValue))
            {
                if (newValue == null)
                    return false;

                currentValue = newValue;
                NotifyPropertyChanged(propertyName);
                return true;
            }

            return false;
        }
    }
}
