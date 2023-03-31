using JsonViewComparer.Enums;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JsonViewComparer.Entities;

internal abstract class JWCBase : IJWCBase, INotifyPropertyChanged
{
    private JWCState state;

    public string Key { get; }

    public JWCState State
    {
        get => this.state;
        private set
        {
            this.state = value;
            this.NotifyPropertyChanged();
        }
    }

    public JWCBase(string key)
    {
        this.Key = key;
    }

    public virtual void SetState(JWCState state)
    {
        this.State = state;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}