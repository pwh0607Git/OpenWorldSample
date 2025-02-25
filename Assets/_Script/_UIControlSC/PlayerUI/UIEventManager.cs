using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventManager : MonoBehaviour
{
    public static UIEventManager Instance { get; private set; }

    private void Awake(){
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // 아이템 아이콘, 타겟 슬롯, 이벤트 종류    
    // target이 아이콘일 가능성을 염두해두고 target slot을 따로 추출한다.
    // (드래그한 아이콘, 이동 할 슬롯)
    public void HandleItemDrop(ItemIconController itemIcon, PointerEventData eventData){
        DragAndDropSlot originalSlot = itemIcon.originalSlot;
        DragAndDropSlot targetSlot = eventData.pointerEnter?.GetComponentInParent<DragAndDropSlot>();
        
        Debug.Log($"Event : HandleItemDrop {itemIcon}, {originalSlot}, {targetSlot}");
        
        if(targetSlot == null){
            itemIcon.ResetToOriginalSlot();
            return;
        }

        if(originalSlot is InventorySlot && targetSlot is ActionBarSlot){
            ItemData itemData = itemIcon.GetComponent<ItemDataHandler>().GetItem;
            Debug.Log($"Event 1");
            if(itemData is Consumable){
                DuplicateIcon(itemIcon, targetSlot);
            }else{
                itemIcon.ResetToOriginalSlot();
            }
        }
        else if(originalSlot is InventorySlot && targetSlot is InventorySlot){
            Debug.Log($"Event 2");
            HandleIcon(itemIcon, originalSlot, targetSlot);
        }
        else if(originalSlot is ActionBarSlot && targetSlot is InventorySlot){
            Debug.Log($"Event 3");
            Destroy(itemIcon.gameObject);
        }
        else if(originalSlot is ActionBarSlot && targetSlot is ActionBarSlot){
            Debug.Log($"Event 4");
            HandleIcon(itemIcon, originalSlot, targetSlot);
        }
    }

    //이 코드를 사용하여 모든 이벤트에서 slot을 기준으로 찾기! 아이콘이 이를 인지해도 slot찾기!!
    private DragAndDropSlot FindVaildDropSlot(GameObject target){
        if(target == null) return null;
        DragAndDropSlot slot = target.GetComponentInParent<DragAndDropSlot>();
        if(slot == null) return null;
        return slot;
    }

    private void HandleIcon(ItemIconController icon, DragAndDropSlot originalSlot, DragAndDropSlot targetSlot){
        if(targetSlot.GetComponentInChildren<ItemIconController>() == null){
            MoveIcon(icon, targetSlot);
        }else{
            SwapIcon(icon, originalSlot, targetSlot);
        }
    }

    private void MoveIcon(ItemIconController icon, DragAndDropSlot targetSlot){
        Debug.Log("Move Icons");
        icon.transform.SetParent(targetSlot.transform);
        targetSlot.AssignCurrentItem(icon.gameObject);
    }

    private void SwapIcon(ItemIconController icon, DragAndDropSlot originalSlot, DragAndDropSlot targetSlot){
        // 순서 => 타겟 슬롯의 아이템을 originalSlot으로 이동 -> icon을 targetSlot에 이동.
        Debug.Log("Swap Icons");
        ItemIconController icon2 = targetSlot.GetComponentInChildren<ItemIconController>();
        icon2.transform.SetParent(originalSlot.transform);
        originalSlot.AssignCurrentItem(icon2.gameObject);
        
        icon.transform.SetParent(targetSlot.transform);
        targetSlot.AssignCurrentItem(icon.gameObject);
    }

    private void DuplicateIcon(ItemIconController icon, DragAndDropSlot targetSlot){
        ItemData itemData = icon.GetComponent<ItemDataHandler>().GetItem;
        if(itemData == null) return;
        if(itemData is Consumable consumable && !consumable.isPresetting) {
            ItemIconController tmpIcon = targetSlot.GetComponentInChildren<ItemIconController>();
        
            if(tmpIcon != null) Destroy(tmpIcon.gameObject);
        
            ItemIconController duplicateItem = Instantiate(icon, targetSlot.transform);
            targetSlot.AssignCurrentItem(duplicateItem.gameObject);
            consumable.isPresetting = true;
        }
        icon.ResetToOriginalSlot();
    }
}