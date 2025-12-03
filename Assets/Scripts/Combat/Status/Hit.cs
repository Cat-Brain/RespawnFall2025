using System;
using UnityEngine;

[Serializable]
public struct Hit
{
    public Vector2 position;
    public float damage;
    public HitStatus[] statuses;
}
