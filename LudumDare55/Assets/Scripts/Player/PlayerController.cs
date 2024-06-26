using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerAttributes playerAttributes;
    [SerializeField] ItemAttrubutes startingWeapon;
    [SerializeField] GameObject iventory;
    [SerializeField] Image[] icons;
    [SerializeField] DamageEnemy meleeAttack;
    [SerializeField] GameObject LoseScreen;
    [SerializeField] Slider healthbar;
    [SerializeField] TextMeshProUGUI summonTracker;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Image summonIconDisplay;

    [Range(0, 1)]
    [SerializeField] float meeleeVolume;
    [Range(0, 1)]
    [SerializeField] float magicVolume;
    [Range(0, 1)]
    [SerializeField] float bowVolume;

    Rigidbody2D playerRigidbody;
    SpriteRenderer playerSpriteRenderer;
    Animator animator;
    Camera cam;

    Health health;
    PlayerMovement playerMovement;
    ItemTracker itemTracker;
    PlayerWeapon playerWeapon;
    WeaponAttributes weaponAttributes;

    float weaponCoolDwonTimer;
    short currentSummonAmount;
    GameObject currentSummonAbility;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        cam = Camera.main;

        health = new(playerAttributes.health, DoAtDeath, DoAtDamage,0, healthbar);
        playerMovement = new();
        itemTracker = new (transform, this, icons);

        ItemHandler(startingWeapon, null);

        weaponCoolDwonTimer = 0;
        currentSummonAmount = 0;
        currentSummonAbility = playerAttributes.DefualtSpawn;

        summonTracker.text = currentSummonAmount.ToString() + "/" + (playerAttributes.maxSummons + itemTracker.GetSummonCapacityBonus());
    }
    private void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            bool exists = animator.parameters.Any(x => x.name == "isWalking");

            if (exists == true)
            {
                animator.SetBool("isWalking", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && currentSummonAmount + 1 <= playerAttributes.maxSummons + itemTracker.GetSummonCapacityBonus() && weaponCoolDwonTimer <= 0 + itemTracker.GetSpeedBonus())
        {
            AddSummon(currentSummonAbility);
            weaponCoolDwonTimer += 1f;
        }
        if (Input.GetKeyDown(KeyCode.B) && iventory.activeInHierarchy == false)
        {
            iventory.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.B) && iventory.activeInHierarchy) 
        {
            iventory.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Space) && weaponCoolDwonTimer <= 0 + itemTracker.GetSpeedBonus())
        {
            try
            {            
                if (DisplayItemData(0).isMagic)
                {
                    playerWeapon.Attack(itemTracker.GetDamageBonus(), DisplayItemData(0).ammo, meleeAttack);

                    AudioSource tempAudio = Instantiate(audioSource, transform.position, Quaternion.identity);
                    tempAudio.clip = playerAttributes.MagicLayer3;
                    tempAudio.Play();
                    tempAudio.volume = magicVolume;

                    tempAudio = Instantiate(audioSource, transform.position, Quaternion.identity);
                    tempAudio.clip = playerAttributes.MagicLayer1[Random.Range(0, playerAttributes.MagicLayer1.Length - 1)];
                    tempAudio.Play();
                    tempAudio.volume = magicVolume;

                    tempAudio = Instantiate(audioSource, transform.position, Quaternion.identity);
                    tempAudio.clip = playerAttributes.MagicLayer2[Random.Range(0, playerAttributes.MagicLayer2.Length - 1)];
                    tempAudio.Play();
                    tempAudio.volume = magicVolume;
                }
                else 
                {
                    playerWeapon.Attack(itemTracker.GetDamageBonus(), DisplayItemData(4).ammo, meleeAttack);

                    AudioSource tempAudio = Instantiate(audioSource, transform.position, Quaternion.identity);
                    tempAudio.clip = playerAttributes.bowShots[Random.Range(0, playerAttributes.bowShots.Length - 1)];
                    tempAudio.Play();
                    tempAudio.volume = bowVolume;
                }
            }
            catch 
            {
                playerWeapon.Attack(itemTracker.GetDamageBonus(), null, meleeAttack);

                AudioSource tempAudio = Object.Instantiate(audioSource, transform.position, Quaternion.identity);
                tempAudio.clip = playerAttributes.swordSwing[Random.Range(0, playerAttributes.swordSwing.Length - 1)];
                tempAudio.Play();
                tempAudio.volume = meeleeVolume;
            }
            weaponCoolDwonTimer = weaponAttributes.attackSpeed;
        }
        else if(weaponCoolDwonTimer > 0 + itemTracker.GetSpeedBonus())
        {
            weaponCoolDwonTimer -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        playerMovement.Movement(playerRigidbody, playerAttributes, animator);
        playerMovement.RotatePlayer(cam, transform);
    }
    //Summon related methods
    public void RemoveSummon() 
    {
        currentSummonAmount--;
        summonTracker.text = currentSummonAmount.ToString() + "/" + (playerAttributes.maxSummons + itemTracker.GetSummonCapacityBonus());
    }
    public void AddSummon(GameObject summon) 
    {
        StartCoroutine(SpawnSummon(summon));
        currentSummonAmount++;
        summonTracker.text = currentSummonAmount.ToString() + "/" + (playerAttributes.maxSummons + itemTracker.GetSummonCapacityBonus());

        AudioSource audio = Instantiate(audioSource, transform.position, Quaternion.identity);
        audio.clip = playerAttributes.summonSound;
        audio.Play();
    }
    public void ChangeSummon(GameObject currentSummonAbility, Sprite summonIcon) 
    {
        this.currentSummonAbility = currentSummonAbility;
        summonIconDisplay.sprite = summonIcon;
    }
    IEnumerator SpawnSummon(GameObject summon)
    {
        yield return new WaitForSeconds(.6f);
        /*Vector3 mousePos = Input.mousePosition;
        mousePos = cam.ScreenToWorldPoint(mousePos);
        
        Instantiate(summon, mousePos + offset, Quaternion.identity);*/
        Vector3 offset = new(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        Instantiate(summon, transform.position + offset, Quaternion.identity);
    }
    public void PlayerFootsteps()
    {
        AudioSource tempAudio = Instantiate(audioSource, transform.position, Quaternion.identity);
        tempAudio.clip = playerAttributes.walkingSound[Random.Range(0, playerAttributes.walkingSound.Length - 1)];
        tempAudio.Play();
    }
    //item related methods
    public void ItemHandler(ItemAttrubutes itemAttrubutes, GameObject itemObject)
    {
        itemTracker.AddItem(itemAttrubutes, itemObject);

        if (itemAttrubutes.itemKey == 1) 
        {
            playerSpriteRenderer.sprite = itemAttrubutes.playerSprite;
        }

        if (itemAttrubutes.isWeapon)
        {
            weaponAttributes = itemAttrubutes.weaponAttributes;
            playerWeapon = new(weaponAttributes, transform, playerAttributes, audioSource);
        }

        summonTracker.text = currentSummonAmount.ToString() + "/" + (playerAttributes.maxSummons + itemTracker.GetSummonCapacityBonus());
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
        health = new(playerAttributes.health, DoAtDeath, DoAtDamage, bonus, healthbar,health.GetHealth());
    }
    public void TakeDamage(float dmg)
    {
        health.TakeDamage(dmg);
    }
    void DoAtDeath()
    {
        LoseScreen.SetActive(true);
        gameObject.SetActive(false);
    }
    void DoAtDamage() 
    {
        
    }

}
