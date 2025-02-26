using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionBarSlot : DragAndDropSlot
{
    [SerializeField] private KeyCode assignedKey;
    public void SetAssigneKey(KeyCode key) => assignedKey = key;
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
        if (IsItemAlreadyInActionBar(item)) return;
        base.AssignCurrentItem(item);
        OnActionBarUpdated?.Invoke(assignedKey, item);
    }
    
    //임시용 코드
    private bool IsItemAlreadyInActionBar(GameObject item)
    {
        //나중에 프리세팅으로 수정할 예정.
        foreach (var slot in FindObjectsOfType<ActionBarSlot>())
        {
            if (slot.currentItem != null && slot.currentItem.GetComponent<ItemDataHandler>().GetItem == item.GetComponent<ItemDataHandler>().GetItem)
            {
                return true;
            }
        }
        return false;
    }

    protected override bool IsValidItem(GameObject item)
    {
        return item.GetComponent<ItemDataHandler>()?.GetItem.itemType == ItemType.Consumable;
    }
}