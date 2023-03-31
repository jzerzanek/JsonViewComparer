using System.Collections.Generic;
using JsonViewComparer.Enums;

namespace JsonViewComparer.Entities;

internal class JWCObject : JWCBase
{
    public List<JWCBase> Items { get; set; }

    public JWCObject(string key) : base(key)
    {
        this.Items = new List<JWCBase>();
    }

    public override void SetState(JWCState state)
    {
        base.SetState(state);

        foreach (var item in this.Items)
        {
            item?.SetState(state);
        }
    }
}