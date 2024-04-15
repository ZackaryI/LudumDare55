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
    private float counterDamageSeconds = 0f;
    /// <summary>
    /// To be fixed by parent Component
    /// </summary>
    public float damage = 1f; 
    private void OnTriggerEnter2D(Collider2D collision)
    { 
 
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        counterDamageSeconds += Time.deltaTime; 
        if(counterDamageSeconds > 0.2f)
        {
            if (collision.GetComponent<EnemyController>())
            { 
                collision.GetComponent<EnemyController>().OnHit(damage, false);
                counterDamageSeconds = 0f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyController>())
        {
            collision.GetComponent<EnemyController>().OnHit(damage, false);
            counterDamageSeconds = 0f;
        } 
    }
}
