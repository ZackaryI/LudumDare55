using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Attributes", menuName = "Weapon Attributes", order = 2)]
public class WeaponAttributes : ScriptableObject
{
    public bool isMelee;

    public float damage;
    public float attackSpeed;
    public float projectileSpeed;

    public Transform projectile;
}
