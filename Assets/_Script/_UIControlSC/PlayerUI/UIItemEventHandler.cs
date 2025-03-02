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
        DragAndDropSlot originalSlot = item.GetComponentInParent<ItemIconController>().originalSlot;
        DragAndDropSlot targetSlot = slot;

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
                else if(targetSlot is ActionBarSlot) DuplicateIcon(targetSlot, item);
            }
        }else{
            Debug.Log("event 3");
            if(originalSlot.GetType() == targetSlot.GetType() || (originalSlot is InventorySlot && targetSlot is EquipmentSlot) || (originalSlot is EquipmentSlot && targetSlot is InventorySlot)) SwapIcon(slot, item);
        }
    }

    static void MoveIcon(DragAndDropSlot targetSlot, GameObject item){
        Debug.Log("아이콘 Move");
        DragAndDropSlot originalslot = item.GetComponentInChildren<ItemIconController>().originalSlot;

        originalslot.ClearSlot();
        targetSlot.SetItem(item);
    }

    static void SwapIcon(DragAndDropSlot targetSlot, GameObject item){
        Debug.Log("아이콘 Swap");
        DragAndDropSlot originalSlot = item.GetComponentInChildren<ItemIconController>().originalSlot;
        GameObject originalItem = targetSlot.GetItem();

        originalSlot.SetItem(originalItem);
        targetSlot.SetItem(item);
        
        // view에 해당 슬롯 데이터를 전달한다.
        // 데이터 동기화 요청
    }

    static void DuplicateIcon(DragAndDropSlot targetSlot, GameObject item){
        Debug.Log("아이콘 Duplicate");
        // icon이 복사되는 조건은 오직 Inventory -> actionbar
        if(item.GetComponentInChildren<ItemDataHandler>().GetItem is Consumable consumable){
            if(consumable.isPresetting == true) return;
        }
        GameObject newIcon = Instantiate(item, targetSlot.transform);
        targetSlot.SetItem(newIcon);
    }
}