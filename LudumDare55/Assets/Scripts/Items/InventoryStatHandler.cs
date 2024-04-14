using TMPro;
using UnityEngine;

public class InventoryStatHandler : MonoBehaviour
{
    [SerializeField] Vector3 offset;

    [SerializeField] TextMeshProUGUI damageBonusText;
    [SerializeField] TextMeshProUGUI speedBonusText;
    [SerializeField] TextMeshProUGUI hpBonusText;
    [SerializeField] TextMeshProUGUI summonCapacityBonusText;

    private void LateUpdate()
    {
        transform.position = Input.mousePosition + offset;
    }

    public void UpdateStats(ItemAttrubutes itemData) 
    {
        damageBonusText.text = itemData.damageBonus.ToString();
        speedBonusText.text = itemData.speedBonus.ToString();
        hpBonusText.text = itemData.hpBonus.ToString();
        summonCapacityBonusText.text = itemData.summonCapacityBonus.ToString();
    }
}
