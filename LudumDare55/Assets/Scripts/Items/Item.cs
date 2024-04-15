using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] ItemAttrubutes itemAttrubutes;
    bool isPlayerInRange;

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
        damageBonusText.text = itemAttrubutes.damageBonus.ToString();
        speedBonusText.text = itemAttrubutes.speedBonus.ToString();
        hpBonusText.text = itemAttrubutes.hpBonus.ToString();
        summonCapacityBonusText.text = itemAttrubutes.summonCapacityBonus.ToString();
    }
    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E)) 
        {
            playerController.ItemHandler(itemAttrubutes, gameObject);          
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

        damageBonusTextEquiped.text = itemData.damageBonus.ToString();
        speedBonusTextEquiped.text = itemData.speedBonus.ToString();
        hpBonusTextEquiped.text = itemData.hpBonus.ToString();
        summonCapacityBonusTextEquiped.text = itemData.summonCapacityBonus.ToString();
        displayStatsEquiped.SetActive(isDisplayed);
    }
}
