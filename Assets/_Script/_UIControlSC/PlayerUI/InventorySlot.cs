using UnityEngine;
public class InventorySlot : DragAndDropSlot
{
    public bool CheckConsumableItem(GameObject item)
    {
        return base.CheckVaildItem<ItemType>(item, ItemType.Consumable);
    }

    public override void AssignCurrentItem(GameObject item){
        base.AssignCurrentItem(item);
        OnChangedItem?.Invoke();
    }
}