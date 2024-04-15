using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SummonController : MonoBehaviour
{

    public LayerMask layers;
    [Header("Enemy Detection Range")]
    private float currentRadius = 25f; 
    [Range(0, 100f)]
    public float radiusCap = 25f; 
    [SerializeField]
    EnemyController target;
    AIDestinationSetter aIDestination;
    [SerializeField]
    EnemyTypeSO summonTypeSO;

    private string summonName;
    [Header("Stats")]
    //Determine si l'ennemi est melee ou ranged, et sa distance minimale pour attaquer
    [SerializeField, Range(0, 25f)]
    public float summonRangeOfAttack;
    private float summonDamage;
    public float summonHP;
    public int summonTier = 1;
    private float delayAttack = 1f;
    private float time = 0f;
    [Header("Colliders")]
    private Collider2D colliderRange;
    public Collider2D colliderAttack;
    [Header("Animation")]
    public Animator animator;
    [Header("Events")]
    /// <summary>
    /// if the enemy attacks. 
    /// The object is already cleaned with the method OnDeath, this event is for additional events outside the class. 
    /// </summary>
    public UnityEvent onAttackEvent;
    /// <summary>
    /// if the summon dies. 
    /// The object is already cleaned with the method OnDeath, this event is for additional events outside the class. 
    /// </summary>
    public UnityEvent onDeathEvent;
    /// <summary>
    /// if the summon receives damage. 
    /// The damage is already called with the method OnHit, this event is for additional events outside the class. 
    /// </summary>
    public UnityEvent onHitEvent;
    /// <summary>
    /// if the summon has a change to its healthbar. 
    /// The damage is already called with the method OnHit, this event is for additional events outside the class. 
    /// </summary>
    public IntUnityEvent onHealthChange; 
    /// <summary>
    /// Used to know the character position
    /// </summary>
    private PlayerController charRef;
    /// <summary>
    /// Used to check if the enemy is currently in the dying animation.
    /// </summary>
    private bool isDying = false;
    /// <summary>
    /// Used to regulate delay attack. 
    /// </summary>
    private bool canAttack = false;
    private AIPath aiPath; 

    private void Initialize()
    {
        aiPath =  GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        summonName = summonTypeSO.enemyName;
        summonRangeOfAttack = summonTypeSO.enemyRangeOfAttack;
        summonHP = summonTypeSO.enemyHP * (summonTier * 0.5f) * 1.2f;
        summonDamage = summonTypeSO.enemyAttack * (summonTier * 0.5f) * 1.2f;
        onDeathEvent.AddListener(() => onDeath());
    }
    private void Start()
    {
        Initialize();
        colliderRange = GetComponent<Collider2D>();
        if (GetComponentInChildren<SummonDamageOnEnter>())
        {
            GetComponentInChildren<SummonDamageOnEnter>().damage = summonDamage;
        }
        //Reference to track Player
        if (charRef == null)
        {
            charRef = FindAnyObjectByType<PlayerController>();
        }
        
        aIDestination = gameObject.GetComponent<AIDestinationSetter>();     
    }

    private void Update()
    {
        //Detect Enemy and go to it 
        senseEnemiesNearby(currentRadius);

        //Loop for attack 
        //We verify that the target is actually alive and active 
        if (target != null && target.gameObject.activeInHierarchy && target.enemyHP >=0)
        {

            Vector2 direction = target.transform.position - transform.position;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
            checkDistance();
            if (canAttack == false)
            { 
                time += Time.deltaTime;

                if (time > delayAttack)
                {
                    time = 0;
                    canAttack = true;
                }
            }
        } 
        //if he's not, we go back to player
        else
        {
            aIDestination.target = charRef.transform;
            Vector2 direction = charRef.transform.position - transform.position;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        }
    }

    public void senseEnemiesNearby(float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, layers); 
        EnemyController enemyTarget = GetClosestEnemy(hitColliders, radius);
        if (enemyTarget != null)
        {
            target = enemyTarget;
            float dist = Vector3.Distance(charRef.transform.position, transform.position);
            if (dist <= summonRangeOfAttack)
            {
                aIDestination.target = null;
                aiPath.canMove = false;
                aiPath.canMove = true;
            } else
            {
                aIDestination.target = enemyTarget.gameObject.transform;
            }
        } else
        { 
            if ( radius <= radiusCap)
            {
                currentRadius += currentRadius;
            }
        }
    }

    private EnemyController GetClosestEnemy(Collider2D[] enemies, float radius)
    {
        EnemyController cub = null;
        float minDist = radius;
        Vector3 currentPos = transform.position;
        foreach (Collider2D c in enemies)
        {
            if (c.gameObject == gameObject && c.gameObject.activeInHierarchy == true)
                continue;
            EnemyController EnemyController = c.GetComponent<EnemyController>();
            if (EnemyController != null && EnemyController.enemyHP > 0)
            {
                Vector3 t = c.transform.position - currentPos;
                float dist = t.x * t.x + t.y * t.y + t.z * t.z;  // Same as "= t.sqrMagnitude;" but faster
                if (dist < minDist)
                {
                    cub = EnemyController;
                    minDist = dist;
                }
            }
        }
        return cub;
    }

 
    /// <summary>
    /// If the enemy meets another collider
    /// Interactions available : A projectile, a summon/the character...
    /// </summary>
    /// <param name="collision">Object colliding with the enemy</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    public void OnHit(float damage)
    {
        summonHP -= damage;
        onHitEvent?.Invoke();
        onHealthChange?.Invoke((int)damage);

        if (summonHP < 0)
        {
            charRef.RemoveSummon();
            onDeathEvent?.Invoke();
        }
    }
    public void checkDistance()
    {

        float dist = Vector3.Distance(charRef.transform.position, transform.position);
        if (dist <= summonRangeOfAttack & canAttack == true)
        {
            aIDestination.target = null;
            aiPath.canMove = false;
            aiPath.canMove = true;
            canAttack = false;
            StartCoroutine(AttackLoop());
        }
    }
    void OnDrawGizmosSelected()
    { 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, currentRadius);
    }

    IEnumerator AttackLoop()
    {
        onAttackEvent?.Invoke();
        if (colliderAttack != null && isDying == false)
        {
            colliderAttack.enabled = true;
        }
        aiPath.canMove = false;
        //animator.SetTrigger("Attack");
        if (summonTypeSO.prefabProjectiles.Count > 0 && isDying == false)
        {
            GameObject o = Instantiate(summonTypeSO.prefabProjectiles[summonTier - 1]);
            o.GetComponent<Projectile>().enemyProjectile = false;
            o.GetComponent<Projectile>().projectileDamage = summonDamage; 
            o.GetComponent<Projectile>().target = target.transform;
            o.transform.position = gameObject.transform.localPosition;
        }
        //Waiting on anim end
        //yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        //allow the enemy to move again
        aiPath.canMove = true;
        yield return new WaitForSeconds(0.2f);
        if (colliderAttack != null && isDying == false)
        {
            colliderAttack.enabled = false;
        }
    }

    void onDeath()
    {

        if (isDying == false)
        { 
            //If we're doing score, add increment to score there 
            isDying = true;

            aiPath.canMove = false;
            animator.SetTrigger("Death");
            StartCoroutine(Despawn());
        }

    }

    IEnumerator Despawn()
    {
        //We let the animation finished first 
        //yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        yield return new WaitForSeconds(2f);
        //We reset the stats of the object to reuse it later through the object pool 
        Initialize();
        gameObject.SetActive(false);
    }
}

[Serializable]
public class IntUnityEvent : UnityEvent<int>
{
}