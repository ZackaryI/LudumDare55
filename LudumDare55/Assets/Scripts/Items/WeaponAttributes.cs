using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Attributes", menuName = "Weapon Attributes", order = 2)]
public class WeaponAttributes : ScriptableObject
{
    public float isRanged;
    public float isMelee;
    public float isMagic;

    public float Damage;

    public Rigidbody2D projectile;
}
