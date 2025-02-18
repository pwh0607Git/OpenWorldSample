using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    public GameObject inventoryWindow;
    [SerializeField] Transform scrollContent;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] GameObject iconBasePrefab;
    
    private List<InventorySlot> slots = new List<InventorySlot>();

    public void SetActive(bool isActive)
    {
        inventoryWindow.SetActive(isActive);
    }
    
    public void CreateSlots(int maxSlotSize){
        for(int i=0;i<maxSlotSize;i++){
            GameObject slotInstance = Instantiate(slotPrefab, scrollContent);
        }
    }

    public void UpdateView(Dictionary<int, ItemData> items){
        for(int i=0;i<items.Count; i++){
            MakeItemIcon(items[i], slots[i]);
        }    
    }

    //인덱스에 걸맞는 슬롯에 아이템 추가하기.
    public void MakeItemIcon(ItemData item, InventorySlot slot){
        GameObject itemIcon = Instantiate(iconBasePrefab, slot.transform);
        
        if (item.itemType == ItemType.Consumable)
        {
            itemIcon.AddComponent<ConsumableItemSC>();
        }else if(item.itemType == ItemType.Equipment)
        {
            itemIcon.AddComponent<EquipmentItemSC>();
        }
    }
}
