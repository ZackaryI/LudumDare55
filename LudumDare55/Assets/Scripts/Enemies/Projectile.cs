using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 
public class Projectile : MonoBehaviour
{
    public bool enemyProjectile = true;
    [Header("Variables")]
    public float projectileDamage;
    public int projectileSpeed = 10;
    public float despawnTime = 0.1f;
    private float startTime;
    private float journeyLength;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 Direction;
    [HideInInspector]
    public Transform target; 
    [SerializeField] private Rigidbody2D rb;
    private Collider2D lastCollider;
    [Header("Event")]
    public UnityEvent onCollision;

    void Start()
    {
        StartCoroutine(DespawnOnTimer(10));
        rb = GetComponent<Rigidbody2D>();
        PlayerController pc = FindObjectOfType<PlayerController>();
        if (enemyProjectile)
        {
            target = pc.transform;
        } else if(target == null)
        {
            Despawn(); 
        }
        startTime = Time.time;
        startPos = gameObject.transform.position;
        endPos = target.transform.position;
        // Calculate the journey length.
        Direction = (endPos - startPos).normalized;
        journeyLength = Vector2.Distance(startPos, target.transform.position);
        rb.velocity = Direction * projectileSpeed;
        gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, Direction);
    }

    void Update()
    {

        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(enemyProjectile == true)
        {

            if(collision.gameObject.GetComponent<PlayerController>() != null && collision != lastCollider)
            {
                lastCollider = collision;
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(projectileDamage);
                onCollision?.Invoke();
                StartCoroutine(Despawn());
            } else if(collision.gameObject.GetComponent<SummonController>() != null && collision != lastCollider)
            { 
                lastCollider = collision;
                collision.gameObject.GetComponent<SummonController>().OnHit(projectileDamage);
                onCollision?.Invoke();
                StartCoroutine(Despawn());
            }
            //Projectile colliding with walls
            else if (collision.gameObject.layer == 7)
            {
                StartCoroutine(Despawn());
            } else if(collision.gameObject.GetComponent<EnemyController>() != null)
            {
                //Do nothing
            } 

        } else
        {
            if (collision.gameObject.GetComponent<EnemyController>() != null && collision != lastCollider)
            {
                lastCollider = collision;
                collision.gameObject.GetComponent<EnemyController>().OnHit(projectileDamage);
                onCollision?.Invoke();
                StartCoroutine(Despawn());
            }
            //Projectile colliding with walls
            else if (collision.gameObject.layer == 7)
            {
                StartCoroutine(Despawn());
            } 
            else if (collision.gameObject.GetComponent<SummonController>() != null || collision.gameObject.GetComponent<SummonController>() != null)
            { 
                //do nothing
            } 
        }
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
    IEnumerator DespawnOnTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
