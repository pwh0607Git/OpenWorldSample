using UnityEngine;

public class EquipmentSlot : DragAndDropSlot
{
    void OnDrop(){

    }
    public override bool CheckVaildItem(GameObject item){
        ItemData itemData = item.GetComponentInChildren<ItemDataHandler>().GetItem;
        if(itemData == null) return false;
        return itemData is Equipment;
    }
}