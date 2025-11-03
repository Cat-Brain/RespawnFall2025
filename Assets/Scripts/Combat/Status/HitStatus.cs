using System;
using System.Collections.Generic;

[Serializable]
public struct HitStatus
{
    public Status status;
    public List<StatusComponent> components;
}
