using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionBarSlot : DragAndDropSlot
{
    private KeyCode assignedKey;

    public void SetAssigneKey(KeyCode assignedKey) { this.assignedKey = assignedKey; }

    public void Update()
    {
        if (Input.GetKeyDown(assignedKey))
        {
            if (currentItem != null)
            {
                UseItem();
            }
            else
            {
                Debug.Log("할당 아이템 없음...");
            }
        }
    }

    void UseItem()
    {
        currentItem.GetComponent<ConsumableItemSC>().GetItem.Use();
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