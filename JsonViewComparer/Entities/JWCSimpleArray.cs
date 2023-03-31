using System.Collections.Generic;

namespace JsonViewComparer.Entities;

internal class JWCSimpleArray : JWCBase
{
    public List<object> Values { get; set; }

    public JWCSimpleArray(string key) : base(key)
    {
        this.Values = new List<object>();
    }
}