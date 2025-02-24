using UnityEngine;
using UnityEngine.EventSystems;

public class ActionBarSlot : DragAndDropSlot
{
    [SerializeField] KeyCode assignedKey;

    public void SetAssigneKey(KeyCode key) { assignedKey = key; }
    public void Update()
    {
        if (Input.GetKeyDown(assignedKey) && currentItem != null)
        {
            UseItem();
            
            if (currentItem == null)  // 아이템이 소진되었으면 제거
            {
                ClearCurrentItem();
            }
        }
    }

    void UseItem()
    {
        currentItem.GetComponent<ConsumableItemHandler>().GetItem.Use();
    }

    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
    }

    public bool CheckConsumableItem(GameObject item)
    {
        return base.CheckVaildItem<ItemType>(item, ItemType.Consumable);
    }
}