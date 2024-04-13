using UnityEngine;

[CreateAssetMenu(fileName = "Item Attributes", menuName = "Item Attributes", order = 3)]
public class ItemAttrubutes : ScriptableObject
{
    /// <summary>
    /// Such as 1-helmet, 2-boots, 3-weapon, 4-pants
    /// </summary>
    public byte itemKey;

    public float damageBonus;
    public float speedBonus;
    public float hpBonus;
    public float summonCapacityBonus;

    public bool isWeapon;
    public WeaponAttributes weaponAttributes;

    public Rigidbody2D droppedItem;
}
