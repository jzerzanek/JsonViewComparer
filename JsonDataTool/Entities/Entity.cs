using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JsonDataTool.Entities
{
    public class Entity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected virtual void ExecuteAndNotify(Action action, [CallerMemberName] string propertyName = null)
        {
            action?.Invoke();
            NotifyPropertyChanged(propertyName);
        }
    }
}
