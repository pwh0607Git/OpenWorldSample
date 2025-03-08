using System.Collections.Generic;
using UnityEngine;

public class UIItemEventHandler : MonoBehaviour
{
    public static UIItemEventHandler Instance { get; private set; }
    private static List<ConsumableData> actionbarConponents = new();

    private void Awake(){
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public static void OnChangedSlot(DragAndDropSlot slot, GameObject item){
        Debug.Log("슬롯 이벤트 발생!");
        if(slot == null || item == null) return;
        
        DragAndDropSlot originalSlot = item.GetComponentInParent<ItemIcon>().originalSlot;
        DragAndDropSlot targetSlot = slot;
        ItemData itemData = item.GetComponentInChildren<Item>().data;

        if(!targetSlot.CheckVaildItem(item)){
            Debug.Log("event 1");
            item.GetComponentInChildren<ItemIcon>().ResetToOriginalSlot();   
            return;   
        }

        if(targetSlot.GetItem() == null){
            Debug.Log("event 2");
            if(originalSlot is InventorySlot){
                if(targetSlot is InventorySlot) MoveIcon(targetSlot, item);
                else if(targetSlot is ActionBarSlot)
                {
                    ConsumableData consumableData = (ConsumableData)itemData;
                    if(IsItemInActionBar(consumableData)) item.GetComponent<ItemIcon>().ResetToOriginalSlot(); 
                    else {
                        RegisterActionBarItem(consumableData);
                        DuplicateIcon(targetSlot, item);
                    }
                }
            }
            else if(originalSlot is ActionBarSlot){
                if(targetSlot is InventorySlot){
                    DestroyIcon(item);
                }
                else if(targetSlot is ActionBarSlot){
                    MoveIcon(targetSlot, item);
                }
            }
        }else{
            Debug.Log("event 3");
            if(originalSlot.GetType() == targetSlot.GetType() || (originalSlot is InventorySlot && targetSlot is EquipmentSlot) || (originalSlot is EquipmentSlot && targetSlot is InventorySlot)) SwapIcon(slot, item);
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

    static void DuplicateIcon(DragAndDropSlot targetSlot, GameObject item){
        Debug.Log("아이콘 Duplicate");
        GameObject newIcon = Instantiate(item, targetSlot.transform);
        // 드래그한 아이템 아이콘은 원위치
        item.GetComponentInChildren<ItemIcon>().ResetToOriginalSlot();
        
        targetSlot.SetItem(newIcon,true);
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