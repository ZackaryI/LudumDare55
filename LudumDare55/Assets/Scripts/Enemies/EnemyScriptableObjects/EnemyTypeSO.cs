using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used as default values for an enemy type. 
/// </summary>
[CreateAssetMenu(fileName = "EnemyType", menuName = "ScriptableObjects/Enemy/EnemyType", order = 1)]
public class EnemyTypeSO : ScriptableObject
{
    public string enemyName;
    [Range(0f, 500f)]
    public float enemyHP = 1f;
    [Range(0f, 100f)]
    public float enemyAttack = 1f;
    [Range(0f, 10f)]
    public float enemyMovementSpeed = 1f;
    [Range(0f, 25f)]
    public float enemyRangeOfAttack = 1f;
    /// <summary>
    /// Quick reference to look up type for events (2D Collisions, Instantiate etc...)
    /// </summary>
    public EnemyTypeSO enemyWeakness;
    /// <summary>
    /// The gameObject we want to instantiate, with tiers
    /// </summary>
    public List<GameObject> prefabEnemy;
    /// <summary>
    /// If the enemy is a ranged, add the projectile, with tiers
    /// </summary>
    public List<GameObject> prefabProjectiles;

    public GameObject playerSummon;
    override public string ToString()
    {
        return "Type : " + enemyName + ", HP : " + enemyHP + ", attack : " + enemyAttack + ", speed:" + enemyMovementSpeed + ", " + enemyMovementSpeed + ", weakness:" + enemyWeakness.enemyName ; 
    }
}
