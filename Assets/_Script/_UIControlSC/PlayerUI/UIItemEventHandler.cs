using System.Collections.Generic;
using UnityEngine;

public class UIItemEventHandler : MonoBehaviour
{
    public static UIItemEventHandler Instance { get; private set; }
    private static List<ConsumableData> actionbarConponents = new();
    [SerializeField] ItemInfoPopup itemPopup;

    private void Awake(){
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public static void OnChangedSlot(DragAndDropSlot slot, GameObject itemIcon){
        Debug.Log("슬롯 이벤트 발생!");
        if(slot == null || itemIcon == null) return;
        
        DragAndDropSlot originalSlot = itemIcon.GetComponentInParent<ItemIcon>().originalSlot;
        DragAndDropSlot targetSlot = slot;
        ItemData itemData = itemIcon.GetComponentInChildren<ItemIcon>().item.data;

        if(!targetSlot.CheckVaildItem(itemIcon)){
            Debug.Log("event 1");
            itemIcon.GetComponentInChildren<ItemIcon>().ResetToOriginalSlot();   
            return;   
        }

        if(targetSlot.GetItem() == null){
            Debug.Log("event 2");
            if(originalSlot is InventorySlot){
                if(targetSlot is InventorySlot) MoveIcon(targetSlot, itemIcon);
                else if(targetSlot is ActionBarSlot)
                {
                    ConsumableData consumableData = (ConsumableData)itemData;
                    if(IsItemInActionBar(consumableData)) itemIcon.GetComponent<ItemIcon>().ResetToOriginalSlot(); 
                    else {
                        RegisterActionBarItem(consumableData);
                        DuplicateIcon(targetSlot, itemIcon);
                    }
                }else if(targetSlot is EquipmentSlot){
                    EquipmentData equipmentData = (EquipmentData)itemData;

                    //슬롯에 아이템이 있는지 없는지 확인하기
                    if(slot.GetItem() == null) MoveIcon(slot, itemIcon);
                    else SwapIcon(slot, itemIcon);
                }
            }
            else if(originalSlot is ActionBarSlot){
                if(targetSlot is InventorySlot){
                    DestroyIcon(itemIcon);
                }
                else if(targetSlot is ActionBarSlot){
                    MoveIcon(targetSlot, itemIcon);
                }
            }
        }else{
            Debug.Log("event 3");
            if(originalSlot.GetType() == targetSlot.GetType() || (originalSlot is InventorySlot && targetSlot is EquipmentSlot) || (originalSlot is EquipmentSlot && targetSlot is InventorySlot)) SwapIcon(slot, itemIcon);
        }
    }

    static void MoveIcon(DragAndDropSlot targetSlot, GameObject item){
        Debug.Log("아이콘 Move");
        DragAndDropSlot originalslot = item.GetComponentInChildren<ItemIcon>().originalSlot;

        originalslot.ClearSlot(true);
        targetSlot.SetItem(item, true);
    }

    static void SwapIcon(DragAndDropSlot targetSlot, GameObject item){
        Debug.Log("아이콘 Swap");
        DragAndDropSlot originalSlot = item.GetComponentInChildren<ItemIcon>().originalSlot;
        GameObject originalItem = targetSlot.GetItem();

        originalSlot.SetItem(originalItem, true);
        targetSlot.SetItem(item, true);
    }

    static void DuplicateIcon(DragAndDropSlot targetSlot, GameObject originalIcon){
        Debug.Log("아이콘 Duplicate");
        ItemIcon targetIcon = originalIcon.GetComponent<ItemIcon>();
        ItemIcon newIcon = UIIconFactory.Instance.CreateItemIcon(targetIcon.item);// Instantiate(originalIcon, targetSlot.transform).GetComponent<ItemIcon>();
        
        if (originalIcon.GetComponentInChildren<ItemIcon>().item is Consumable consumable)
        {
            consumable.SubscribeToUseEvent(newIcon.UpdateIcon);
        }

        originalIcon.GetComponentInChildren<ItemIcon>().ResetToOriginalSlot();
        targetSlot.SetItem(newIcon.gameObject, true);
    }

    static void DestroyIcon(GameObject item){
        ItemData itemData = item.GetComponentInChildren<Item>().data;
        item.GetComponentInChildren<ItemIcon>().originalSlot.ClearSlot(true);
        
        if(itemData != null && itemData is ConsumableData consumable){
            UnregisterActionBarItem(consumable);
        }
        Destroy(item);
    }

    private static void RegisterActionBarItem(ConsumableData data)
    {
        if(!IsItemInActionBar(data))
            actionbarConponents.Add(data);
    }

    private static void UnregisterActionBarItem(ConsumableData data)
    {
        actionbarConponents.Remove(data);
    }

    private static  bool IsItemInActionBar(ConsumableData data)
    {
        return actionbarConponents.Contains(data);
    }
}


// ItemData[고정 데이터] -> Item[ 동적 데이터 처리 ] -> ItemIconController[아이콘 ui 상호작용.cs]