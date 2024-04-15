using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    public float damage;

    private void Start()
    {
        Destroy(gameObject, 1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyController>()) 
        {
            collision.GetComponent<EnemyController>().OnHit(damage, false);
        }
    }
}
