using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class ProjectileTrail : MonoBehaviour
{
    public LineRenderer lr;

    public float length, oscillationFrequency, travelFrequencyMultiplier, oscillationAmplitude;

    public List<Vector2> positions;
    public float distanceTraveled;

    private void Awake()
    {
        positions = new List<Vector2>();
        lr.positionCount = 0;
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            if (positions.Count != 100)
                positions = new Vector2[100].ToList();
            for (int i = 0; i < 100; i++)
                positions[i] = Vector2.Lerp((Vector2)transform.position - Vector2.right * length,
                    transform.position, (float)i / 99f);
        }
        else
        {
            if (positions.Count <= 1)
            {
                lr.positionCount = 0;
                return;
            }

            float totalDist = 0;
            for (int i = positions.Count - 2; i >= 0; i--)
            {
                totalDist += Vector2.Distance(positions[i], positions[i + 1]);

                if (totalDist <= length)
                    continue;
                positions.RemoveRange(0, i);
                break;
            }
        }


        float dist = 0;
        Vector3[] finalPositions = new Vector3[positions.Count];
        finalPositions[finalPositions.Length - 1] = positions[positions.Count - 1];
        for (int i = positions.Count - 2; i > 0; i--)
        {
            dist += Vector2.Distance(positions[i], positions[i + 1]);
            finalPositions[i] = OscillationPosition(positions[i], (positions[i + 1] - positions[i - 1]).normalized, dist);
        }
        finalPositions[0] = OscillationPosition(positions[0], (positions[1] - positions[0]).normalized, dist);

        lr.positionCount = positions.Count;
        lr.SetPositions(finalPositions);
        lr.widthMultiplier = 0.5f * (transform.lossyScale.x + transform.lossyScale.y);
    }

    public Vector2 OscillationPosition(Vector2 pos, Vector2 direction, float dist)
    {
        return pos + CMath.Rotate90Clock(direction) * GetOscillation(dist);
    }

    public float GetOscillation(float dist)
    {
        return Mathf.Sin((dist + travelFrequencyMultiplier * distanceTraveled) * oscillationFrequency) * oscillationAmplitude * dist / length;
    }

    private void FixedUpdate()
    {
        if (!Application.isPlaying)
            return;

        if (positions.Count != 0 && positions[positions.Count - 1] == (Vector2)transform.position)
            return;
        positions.Add(transform.position);
        if (positions.Count > 1)
            distanceTraveled += Vector2.Distance(positions[positions.Count - 2], positions[positions.Count - 1]);
    }
}
