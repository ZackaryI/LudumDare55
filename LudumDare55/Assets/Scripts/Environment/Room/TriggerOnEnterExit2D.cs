using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Script to trigger UnityEvent on Trigger2D collision of the player.
/// </summary>
public class TriggerOnEnterExit2D : MonoBehaviour
{
    public UnityEvent eventOnEnter2D;
    public UnityEvent eventOnExit2D; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {

            eventOnEnter2D?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            eventOnExit2D?.Invoke();
        }
    }
}
