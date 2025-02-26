using UnityEngine;
public class InventorySlot : DragAndDropSlot
{
    protected override bool IsValidItem(GameObject item)
    {
        return item.GetComponent<ItemDataHandler>() != null;
    }
}