using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerAttributes playerAttributes;
    [SerializeField] WeaponAttributes weaponAttributes;

    Rigidbody2D playerRigidbody;
    Camera cam;

    Health health;
    PlayerMovement playerMovement;
    ItemTracker itemTracker;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        health = new(playerAttributes.health, DoAtDeath, DoAtDamage, 0);
        playerMovement = new();
        itemTracker = new ItemTracker(transform, this);
    }
    private void FixedUpdate()
    {
        playerMovement.Movement(playerRigidbody, playerAttributes, itemTracker.GetSpeedBonus());
        playerMovement.RotatePlayer(cam, transform);
    }

    //item related methods
    public void ItemHandler(ItemAttrubutes itemAttrubutes)
    {
        if (itemAttrubutes.isWeapon) 
        {
            weaponAttributes = itemAttrubutes.weaponAttributes;
        }

        itemTracker.AddItem(itemAttrubutes);
    }
    public ItemAttrubutes DisplayItemData(byte index) 
    {
        return itemTracker.GetItemData(index);
    }

    //Health class related methods
    public void Heal(float amount)
    {
        health.Heal(amount);
    }
    public void UpdateHealth(float bonus) 
    {
        health = new(playerAttributes.health, DoAtDeath, DoAtDamage, bonus, health.GetHealth());
    }
    public void TakeDamage(float dmg)
    {
        health.TakeDamage(dmg);
    }
    void DoAtDeath()
    {
        Debug.Log("Not implamented");
    }
    void DoAtDamage() 
    {
        Debug.Log("Not implamented");
    }
}
