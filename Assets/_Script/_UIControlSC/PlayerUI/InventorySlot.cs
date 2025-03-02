using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : DragAndDropSlot, IDropHandler
{
    public event Action<SlotData<int>> OnSlotUpdated;
    public int index;
    public override bool CheckVaildItem(GameObject item)
    {
        return base.CheckVaildItem(item);
    }

    public new void SetItem(GameObject item)
    {
        base.SetItem(item);

        ItemData itemData = item.GetComponentInChildren<ItemDataHandler>()?.GetItem;
        OnSlotUpdated?.Invoke(new SlotData<int>(index, itemData));
    }

    public new void ClearSlot()
    {
        base.ClearSlot();
        OnSlotUpdated?.Invoke(new SlotData<int>(index, null));
    }
}