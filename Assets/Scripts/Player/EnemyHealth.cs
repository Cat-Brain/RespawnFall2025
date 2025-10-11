using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Death();
        }
    }

    private void OnTriggerEnter2D(Collider2D playerProjectile)
    {
        if (playerProjectile.gameObject.layer == 8)
        {
            Destroy(playerProjectile.gameObject);
            TakeDamage(1);
        }
    }

    private void TakeDamage(int amount)
    {
        health -= amount;
    }
    
    virtual public void Death()
    {
        Destroy(gameObject);
    }
}
