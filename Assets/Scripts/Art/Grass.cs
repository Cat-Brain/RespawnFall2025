using System;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    public LineRenderer lr;

    public float windFrequency, windStrength, windTimeMultiplier;
    public float movementMultiplier;
    public float springFrequency, springDamping;

    public float rotation = 0, velocity = 0;
    public SpringUtils.tDampedSpringMotionParams spring = new();
    public FastNoiseLite noise;

    public Dictionary<Rigidbody2D, Vector2> trackedRigidbodies = new();

    void Awake()
    {
        noise = new FastNoiseLite((int)((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds());
        noise.SetFrequency(windFrequency);
    }

    void Update()
    {
        velocity += windStrength * Time.deltaTime *
            noise.GetNoise(transform.position.x, Time.timeSinceLevelLoad * windTimeMultiplier);

        SpringUtils.CalcDampedSpringMotionParams(ref spring, Time.deltaTime, springFrequency, springDamping);
        SpringUtils.UpdateDampedSpringMotion(ref rotation, ref velocity, 0, spring);

        lr.SetPosition(0, CMath.Rotate(transform.up, rotation));
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.attachedRigidbody)
        {
            Vector2 oldVel;
            if (trackedRigidbodies.TryGetValue(collision.attachedRigidbody, out oldVel))
                trackedRigidbodies[collision.attachedRigidbody] = collision.attachedRigidbody.linearVelocity;
            else
            {
                oldVel = Vector2.zero;
                trackedRigidbodies.Add(collision.attachedRigidbody, collision.attachedRigidbody.linearVelocity);
            }

            velocity -= Vector2.Dot(
                transform.right, collision.attachedRigidbody.linearVelocity - oldVel) * movementMultiplier;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody)
            trackedRigidbodies.Remove(collision.attachedRigidbody);
    }
}
