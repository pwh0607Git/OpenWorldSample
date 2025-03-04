using UnityEngine;

public class UIItemEventHandler : MonoBehaviour
{
    public static UIItemEventHandler Instance { get; private set; }
    
    private void Awake(){
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public static void OnChangedSlot(DragAndDropSlot slot, GameObject item){
        Debug.Log("슬롯 이벤트 발생!");
        if(slot == null || item == null) return;
        
        DragAndDropSlot originalSlot = item.GetComponentInParent<ItemIconController>().originalSlot;
        DragAndDropSlot targetSlot = slot;
        ItemData itemData = item.GetComponentInChildren<ItemDataHandler>().GetItem;

        // 이벤트가 발생한 슬롯, 슬롯에 들어온 아이템
        if(!targetSlot.CheckVaildItem(item)){
            Debug.Log("event 1");
            item.GetComponentInChildren<ItemIconController>().ResetToOriginalSlot();   
            return;   
        }

        //유효한 아이템 인경우.
        if(targetSlot.GetItem() == null){
            Debug.Log("event 2");
            if(originalSlot is InventorySlot){
                if(targetSlot is InventorySlot) MoveIcon(targetSlot, item);
                else if(targetSlot is ActionBarSlot)
                {
                    bool isPresetting = ((Consumable)itemData).isPresetting;
                    if(isPresetting) item.GetComponent<ItemIconController>().ResetToOriginalSlot(); 
                    else {
                        ((Consumable)itemData).isPresetting = true;
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
        DragAndDropSlot originalslot = item.GetComponentInChildren<ItemIconController>().originalSlot;

        originalslot.ClearSlot(true);
        targetSlot.SetItem(item, true);
    }

    static void SwapIcon(DragAndDropSlot targetSlot, GameObject item){
        Debug.Log("아이콘 Swap");
        DragAndDropSlot originalSlot = item.GetComponentInChildren<ItemIconController>().originalSlot;
        GameObject originalItem = targetSlot.GetItem();

        originalSlot.SetItem(originalItem, true);
        targetSlot.SetItem(item, true);
    }

    static void DuplicateIcon(DragAndDropSlot targetSlot, GameObject item){
        Debug.Log("아이콘 Duplicate");
        GameObject newIcon = Instantiate(item, targetSlot.transform);
        // 드래그한 아이템 아이콘은 원위치
        item.GetComponentInChildren<ItemIconController>().ResetToOriginalSlot();
        
        targetSlot.SetItem(newIcon,true);
    }

    static void DestroyIcon(GameObject item){
        ItemData itemData = item.GetComponentInChildren<ItemDataHandler>().GetItem;
        item.GetComponentInChildren<ItemIconController>().originalSlot.ClearSlot(true);
        
        if(itemData != null && itemData is Consumable consumable){
            consumable.isPresetting = false;
        }

        Destroy(item);

    }
}