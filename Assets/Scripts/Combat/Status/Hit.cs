using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Hit
{
    public Vector2 position;
    public float damage;
    public List<HitStatus> statuses;
}
