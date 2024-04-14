using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerAttributes playerAttributes;
    [SerializeField] WeaponAttributes weaponAttributes;
    [SerializeField] ItemAttrubutes startingWeapon;

    Rigidbody2D playerRigidbody;
    Camera cam;

    Health health;
    PlayerMovement playerMovement;
    ItemTracker itemTracker;
    PlayerWeapon playerWeapon;
    float weaponCoolDwonTimer;

    private void Start()
    {
        ItemHandler(startingWeapon);

        playerRigidbody = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        health = new(playerAttributes.health, DoAtDeath, DoAtDamage, 0);
        playerMovement = new();
        itemTracker = new (transform, this);

        weaponCoolDwonTimer = 0;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && weaponCoolDwonTimer <= 0)
        {
            playerWeapon.Attack();
            weaponCoolDwonTimer = weaponAttributes.attackSpeed;
        }
        else if(weaponCoolDwonTimer > 0)
        {
            weaponCoolDwonTimer -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        playerMovement.Movement(playerRigidbody, playerAttributes, itemTracker.GetSpeedBonus());
        playerMovement.RotatePlayer(cam, transform);
    }

    //item related methods
    public void ItemHandler(ItemAttrubutes itemAttrubutes)
    {
        itemTracker.AddItem(itemAttrubutes);

        if (itemAttrubutes.isWeapon)
        {
            weaponAttributes = itemAttrubutes.weaponAttributes;
            playerWeapon = new(weaponAttributes, itemTracker.GetDamageBonus());
            return;
        }

        playerWeapon.UpdateDamageBonus(itemTracker.GetDamageBonus());
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
