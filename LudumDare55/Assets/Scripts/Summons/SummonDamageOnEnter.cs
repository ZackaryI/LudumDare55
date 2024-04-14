using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
/// <summary>
/// TODO
/// Empty for now till Character Script 
/// </summary>
public class SummonDamageOnEnter : MonoBehaviour
{
    /// <summary>
    /// To be fixed by parent Component
    /// </summary>
    public float damage = 1f; 
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.GetComponent<PlayerController>())
        {
        } else if (collision.GetComponent<SummonController>())
        {

        } else if (collision.GetComponent<EnemyController>())
        {
            Debug.Log("hit melee");
            collision.GetComponent<EnemyController>().OnHit(damage);

        }
    }
}
