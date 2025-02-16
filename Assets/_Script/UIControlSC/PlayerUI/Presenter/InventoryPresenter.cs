using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
    private InventoryModel model;
    private InventoryView view;

    public InventoryPresenter(InventoryModel model, InventoryView view){
        this.model = model;
        this.view = view;
        view.CreateSlots(model.maxSlotSize);
    }

    public void ToggleInventory(){
        bool isActive = !view.inventoryWindow.activeSelf;
        view.SetActive(isActive);

        if(isActive){
            view.UpdateInventoryUI(model.GetItemList());
        }
    }

    public void AddItem(ItemData item)
    {
        if (model.AddItem(item))
        {
            view.UpdateInventoryUI(model.GetItemList());
        }
        else
        {
            Debug.Log("인벤토리 공간 부족");
        }
    }

    public bool CheckSlotSize()
    {
        return model.CheckSlotSize();
    }
}
