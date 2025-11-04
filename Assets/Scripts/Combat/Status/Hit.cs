using System;
using System.Collections.Generic;

[Serializable]
public struct Hit
{
    public float damage;
    public List<HitStatus> statuses;
}
