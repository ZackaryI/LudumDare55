using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject itemStats;
    
    InventoryStatHandler inventoryStatHandler;   
    ItemAttrubutes itemData;
    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        inventoryStatHandler= itemStats.GetComponent<InventoryStatHandler>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemData = playerController.GetItemDataForInventory(image);

        if (itemData != null) 
        {
            itemStats.SetActive(true);
            inventoryStatHandler.UpdateStats(itemData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemStats.SetActive(false);
    }
}
