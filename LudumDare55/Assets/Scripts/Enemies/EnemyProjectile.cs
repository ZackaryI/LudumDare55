using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 
public class EnemyProjectile : MonoBehaviour
{
    [Header("Variables")]
    public int projectileDamage;
    public int projectileSpeed = 10;
    public float despawnTime = 0.1f;
    private float startTime;
    private float journeyLength;
    private Vector3 startPos;
    private Vector3 endPos;

    [SerializeField] private Rigidbody2D rb;
    private Collider2D lastCollider;
    [Header("Event")]
    public UnityEvent onCollision;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CharacterTarget pc = FindObjectOfType<CharacterTarget>();
        startTime = Time.time;
        startPos = gameObject.transform.position;
        endPos = pc.transform.position;
        // Calculate the journey length.
        Vector3 Direction = (endPos - startPos).normalized;
        journeyLength = Vector2.Distance(startPos, pc.transform.position);
        rb.velocity = Direction * projectileSpeed;
        gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, Direction);
    }

    void Update()
    {
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * projectileSpeed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<CharacterTarget>() != null && collision != lastCollider)
        {
            Debug.Log(collision.gameObject.GetComponent<CharacterTarget>());
            lastCollider = collision;
            //collision.transform.parent.gameObject.GetComponent<CharacterTarget>().OnHit(damage);
            onCollision?.Invoke();
            StartCoroutine(Despawn());
        }
        //Projectile colliding with walls
        else if (collision.gameObject.layer == 7)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
}
