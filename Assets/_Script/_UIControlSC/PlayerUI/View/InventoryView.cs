using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    public GameObject inventoryWindow;
    public Transform scrollContent;
    public GameObject slotPrefab;
    
    [SerializeField] Transform startPoint, endPoint;
    [SerializeField] int columns = 4;
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

    public void UpdateInventoryUI(List<ItemData> items){
        for(int i =0 ; i< slots.Count;i++){
            if(i<items.Count){
                slots[i].SetItem(items[i]);
            }else{
                slots[i].ClearSlot();
            }
        }
    }
}
