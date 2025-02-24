using UnityEditor;
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
    public void HandleItemDrop(ItemIconController itemIcon, Transform targetSlotTransform, PointerEventData eventData){
        DragAndDropSlot originalSlot = itemIcon.originalParent.GetComponent<DragAndDropSlot>();
        DragAndDropSlot targetSlot = targetSlotTransform?.GetComponent<DragAndDropSlot>();
        
        if(targetSlot == null){
            itemIcon.ResetToOriginalSlot();
            return;
        }

        if(originalSlot is InventorySlot && targetSlot is ActionBarSlot){
            ItemData itemData = itemIcon.GetComponent<ItemDataHandler>().GetItem;
            if(itemData is Consumable){
                GameObject duplicateItem = Instantiate(itemIcon.gameObject, targetSlotTransform);
                targetSlot.AssignCurrentItem(duplicateItem);
                itemIcon.ResetToOriginalSlot(); // 원본은 그대로 둠
            }else{
                itemIcon.ResetToOriginalSlot();
            }
        }
        else if(originalSlot is ActionBarSlot && targetSlot is InventorySlot){
            Destroy(itemIcon.gameObject);
        }
        else if(originalSlot is ActionBarSlot && targetSlot is ActionBarSlot ){

        }
    }
    //이 코드를 사용하여 모든 이벤트에서 slot을 기준으로 찾기! 아이콘이 이를 인지해도 slot찾기!!
    private DragAndDropSlot FindVaildDropSlot(GameObject target){
        if(target == null) return null;
        DragAndDropSlot slot = target.GetComponentInParent<DragAndDropSlot>();
        if(slot == null) return null;
        return slot;
    }
}
