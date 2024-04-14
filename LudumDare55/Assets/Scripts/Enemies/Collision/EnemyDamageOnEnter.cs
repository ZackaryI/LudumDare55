using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
/// <summary>
/// TODO
/// Empty for now till Character Script 
/// </summary>
public class EnemyDamageOnEnter : MonoBehaviour
{
    /// <summary>
    /// To be fixed by parent Component
    /// </summary>
    public float damage = 1f; 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
        } else if (collision.GetComponent<SummonController>())
        {
            collision.GetComponent<SummonController>().OnHit(damage);

        } else if (collision.GetComponent<EnemyController>())
        {

        }
    }
}
