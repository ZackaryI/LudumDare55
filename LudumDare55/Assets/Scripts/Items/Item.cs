using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ItemAttrubutes itemAttrubutes;
    [SerializeField] bool isPlayerInRange;

    [SerializeField] GameObject displayStats;
    [SerializeField] TextMeshProUGUI damageBonusText;
    [SerializeField] TextMeshProUGUI speedBonusText;
    [SerializeField] TextMeshProUGUI hpBonusText;
    [SerializeField] TextMeshProUGUI summonCapacityBonusText;

    [SerializeField] GameObject displayStatsEquiped;
    [SerializeField] TextMeshProUGUI damageBonusTextEquiped;
    [SerializeField] TextMeshProUGUI speedBonusTextEquiped;
    [SerializeField] TextMeshProUGUI hpBonusTextEquiped;
    [SerializeField] TextMeshProUGUI summonCapacityBonusTextEquiped;

    PlayerController playerController;
    private void Start()
    {
        isPlayerInRange = false;
        damageBonusText.text = "Damage: +" + itemAttrubutes.damageBonus;
        speedBonusText.text = "Speed: +" + itemAttrubutes.speedBonus;
        hpBonusText.text = "Hp: +" + itemAttrubutes.hpBonus;
        summonCapacityBonusText.text = "SummonCap: +" + itemAttrubutes.summonCapacityBonus; 
    }
    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E)) 
        {
            playerController.ItemHandler(itemAttrubutes);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            isPlayerInRange = true;
            playerController = collision.GetComponent<PlayerController>();
            DisplayEquipedData(true);
            displayStats.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            DisplayEquipedData(false);
            displayStats.SetActive(false);
        }
    }
    void DisplayEquipedData(bool isDisplayed) 
    {
        ItemAttrubutes itemData = playerController.DisplayItemData(itemAttrubutes.itemKey);

        if (itemData == null) { return; }

        damageBonusTextEquiped.text = "Damage: +" + itemData.damageBonus;
        speedBonusTextEquiped.text = "Speed: +" + itemData.speedBonus;
        hpBonusTextEquiped.text = "Hp: +" + itemData.hpBonus;
        summonCapacityBonusTextEquiped.text = "SummonCap: +" + itemData.summonCapacityBonus;
        displayStatsEquiped.SetActive(isDisplayed);
    }
}