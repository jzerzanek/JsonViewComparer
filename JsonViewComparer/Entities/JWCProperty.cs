namespace JsonViewComparer.Entities;

internal class JWCProperty : JWCBase
{
    public object Value { get; set; }

    public JWCProperty(string key, object value) : base(key)
    {
        this.Value = value;
    }
}