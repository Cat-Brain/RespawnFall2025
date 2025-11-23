using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int tier;
    public LayerMask playerMask, wallMask;
    public float sightDist;

    public Collider2D FindPlayer()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, sightDist, playerMask))
            if (collider.enabled && PositionVisible(collider.transform.position))
                return collider;
        return null;
    }

    public bool PositionVisible(Vector3 position)
    {
        return !Physics2D.Linecast(transform.position, position, wallMask);
    }
}
