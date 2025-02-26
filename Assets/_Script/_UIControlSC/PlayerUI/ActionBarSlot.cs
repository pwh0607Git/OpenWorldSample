using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionBarSlot : DragAndDropSlot
{
    [SerializeField] KeyCode assignedKey;
    public void SetAssigneKey(KeyCode key) { assignedKey = key; }
    public event Action<KeyCode, GameObject> OnActionBarUpdated;
    public void Update()
    {
        if (Input.GetKeyDown(assignedKey) && currentItem != null)
        {
            UseItem();
            if (currentItem == null) ClearCurrentItem();
        }
    }

    void UseItem()
    {
        currentItem.GetComponent<ConsumableItemHandler>().GetItem.Use();
    }

    public override void AssignCurrentItem(GameObject item){
        base.AssignCurrentItem(item);
        OnActionBarUpdated?.Invoke(assignedKey, item);
    }

    public bool CheckConsumableItem(GameObject item) => base.CheckVaildItem<ItemType>(item, ItemType.Consumable);
}