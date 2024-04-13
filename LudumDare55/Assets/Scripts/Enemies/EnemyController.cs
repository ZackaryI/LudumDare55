using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider2D))]
/// <summary>
/// Control enemy AI : 
/// </summary>
public class EnemyController : MonoBehaviour
{
    [SerializeField]
    EnemyTypeSO enemyTypeSO;

    private string enemyName;
    [Header("Stats")]
    //Determine si l'ennemi est melee ou ranged, et sa distance minimale pour attaquer
    [SerializeField, Range(0, 25f)]
    public float enemyRangeOfAttack; 
    private float enemyDamage;
    public float enemyHP;
    public int enemyTier = 1;
    private float delayAttack = 1f; 
    private float time = 0f;
    [Header("Colliders")]
    private Collider2D colliderRange; 
    public Collider2D colliderAttack;
    [Header("Animation")]
    public Animator animator;
    [Header("Events")]
    /// <summary>
    /// if the enemy dies. 
    /// The object is already cleaned with the method OnDeath, this event is for additional events outside the class. 
    /// </summary>
    public UnityEvent onDeathEvent;
    /// <summary>
    /// if the enemy receives damage. 
    /// The damage is already called with the method OnHit, this event is for additional events outside the class. 
    /// </summary>
    public UnityEvent onHitEvent; 
    private AIPath aiPath; 
    /// <summary>
    /// Used to know the character position
    /// </summary>
    private CharacterTarget charRef; 
    /// <summary>
    /// Used to check if the enemy is currently in the dying animation.
    /// </summary>
    private bool isDying = false;
    /// <summary>
    /// Used to regulate delay attack. 
    /// </summary>
    private bool canAttack = false;

    private void Initialize()
    {
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        enemyName = enemyTypeSO.enemyName;
        enemyRangeOfAttack = enemyTypeSO.enemyRangeOfAttack;
        enemyHP = enemyTypeSO.enemyHP;
        enemyDamage = enemyTypeSO.enemyAttack;
        onDeathEvent.AddListener(() => onDeath()); 
    }
    // Start is called before the first frame update
    void Start()
    {
        colliderRange = GetComponent<Collider2D>(); 
        if(charRef == null)
        {
            charRef = FindAnyObjectByType<CharacterTarget>(); 
        }
        Initialize();
        Debug.Log(enemyHP); 
    }

    // Update is called once per frame
    void Update()
    {

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

    }
    public void checkDistance()
    {

        float dist = Vector3.Distance(charRef.transform.position, transform.position);
        if (dist <= enemyRangeOfAttack & canAttack == true)
        {
            canAttack = false;
            StartCoroutine(AttackLoop());
        }
    }

    IEnumerator AttackLoop()
    {
        if (colliderAttack != null && isDying == false)
        {
            colliderAttack.enabled = true;
        } 
        aiPath.canMove = false; 
        animator.SetTrigger("Attack");
        if (enemyTypeSO.prefabProjectiles.Count > 0 && isDying == false)
        {
            GameObject o = Instantiate(enemyTypeSO.prefabProjectiles[enemyTier-1]);
            o.transform.position = gameObject.transform.localPosition; 
        }
        //Waiting on anim end
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        //allow the enemy to move again
        aiPath.canMove = true;
        yield return new WaitForSeconds(0.2f);
        if (colliderRange != null)
        {
            colliderRange.enabled = false;
        }
    }

    void onDeath()
    {

        if (isDying == false)
        {
            if (colliderRange != null)
                colliderRange.enabled = false;
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
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);
        yield return new WaitForSeconds(2f);
        //We reset the stats of the object to reuse it later through the object pool 
        Initialize();
        gameObject.SetActive(false);
    }
}
