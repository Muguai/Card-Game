using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ZoneStatus 
{
    public virtual IEnumerator Start()
    {
        yield break;
    }

    public virtual IEnumerator Activate()
    {
        yield break;
    }
}
