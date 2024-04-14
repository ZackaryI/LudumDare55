using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemTracker 
{
    ItemBonuses itemBonuses;
    Dictionary<byte, ItemAttrubutes> items;

    Transform playerPosition;
    PlayerController playerController;

    Image[] icons;

    public ItemTracker(Transform playerPosition, PlayerController playerController, Image[] icons) 
    {
        this.playerPosition = playerPosition;
        this.playerController = playerController;
        this.icons = icons;

        itemBonuses = new ();
        items = new ();
    }

    public void AddItem(ItemAttrubutes item) 
    {
        if (items.ContainsKey(item.itemKey)) 
        {
            RemoveItem(item.itemKey);
        }

        items.Add(item.itemKey, item);
        playerController.UpdateHealth(item.hpBonus);
        itemBonuses.UpdateValues(item.damageBonus, item.speedBonus, item.hpBonus, item.summonCapacityBonus);
        
        icons[item.itemKey].sprite = item.Icon;
        icons[item.itemKey].color = new Color(255, 255, 255, 255);
    }
    public void RemoveItem(byte key) 
    {
        itemBonuses.UpdateValues(-items[key].damageBonus, -items[key].speedBonus, -items[key].hpBonus, -items[key].summonCapacityBonus);
        playerController.UpdateHealth(-items[key].hpBonus);
        TossUnequipedItem(items[key].droppedItem);
        icons[key].sprite = null;
        icons[key].color = new Color(255, 255, 255, 0);
        items.Remove(key);
    }
    public void TossUnequipedItem(Transform item)
    {
        Object.Instantiate(item, playerPosition.position, Quaternion.identity);
    }
    public float GetDamageBonus()
    {
        return itemBonuses.DamageBonus;
    }
    public float GetSpeedBonus()
    {
        return itemBonuses.SpeedBonus;
    }
    public float GetHpBonus()
    {
        return itemBonuses.HpBonus;
    }
    public float GetSummonCapacityBonus()
    {
        return itemBonuses.SummonCapacityBonus;
    }
    public ItemAttrubutes GetItemData(byte index) 
    {
        try
        {
            return items[index];
        }
        catch 
        {
            return null;
        }
    }
}
