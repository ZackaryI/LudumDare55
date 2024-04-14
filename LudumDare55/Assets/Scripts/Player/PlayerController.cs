using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerAttributes playerAttributes;
    [SerializeField] WeaponAttributes weaponAttributes;
    [SerializeField] ItemAttrubutes startingWeapon;
    [SerializeField] GameObject iventory;
    [SerializeField] Image[] icons;

    Rigidbody2D playerRigidbody;
    SpriteRenderer playerSpriteRenderer;
    Camera cam;

    Health health;
    PlayerMovement playerMovement;
    ItemTracker itemTracker;
    PlayerWeapon playerWeapon;
    float weaponCoolDwonTimer;


    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        cam = Camera.main;

        health = new(playerAttributes.health, DoAtDeath, DoAtDamage, 0);
        playerMovement = new();
        itemTracker = new (transform, this, icons);

        ItemHandler(startingWeapon);

        weaponCoolDwonTimer = 0;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && iventory.activeInHierarchy == false)
        {
            iventory.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.B) && iventory.activeInHierarchy) 
        {
            iventory.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Space) && weaponCoolDwonTimer <= 0)
        {
            playerWeapon.Attack(itemTracker.GetDamageBonus());
            weaponCoolDwonTimer = weaponAttributes.attackSpeed;
        }
        else if(weaponCoolDwonTimer > 0 + itemTracker.GetSpeedBonus())
        {
            weaponCoolDwonTimer -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        playerMovement.Movement(playerRigidbody, playerAttributes);
        playerMovement.RotatePlayer(cam, transform);
    }

    //item related methods
    public void ItemHandler(ItemAttrubutes itemAttrubutes)
    {
        itemTracker.AddItem(itemAttrubutes);

        if (itemAttrubutes.itemKey == 1) 
        {
            playerSpriteRenderer.sprite = itemAttrubutes.playerSprite;
        }

        if (itemAttrubutes.isWeapon)
        {
            weaponAttributes = itemAttrubutes.weaponAttributes;
            playerWeapon = new(weaponAttributes);
            return;
        }
    }
    public ItemAttrubutes DisplayItemData(byte index) 
    {
        return itemTracker.GetItemData(index);
    }
    public ItemAttrubutes GetItemDataForInventory(Image gameobject) 
    {

        for (byte i = 0; i < icons.Length; i++) 
        {
            if (icons[i] == gameobject) 
            {
                return DisplayItemData(i);    
            }
        }

        return null;
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
