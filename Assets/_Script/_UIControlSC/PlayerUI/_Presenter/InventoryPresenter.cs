using System.Collections.Generic;

public class InventoryPresenter
{
    private InventoryModel model;
    private InventoryView view;

    //View가 비활성화 된 상태일 때 저장하는 코드.
    private List<SlotData<int>> pendingUpdates = new List<SlotData<int>>();

    public InventoryPresenter(InventoryModel model, InventoryView view){
        this.model = model;
        this.view = view;
        view.OnViewUpdated += UpdateModel;
        model.OnModelUpdated += UpdateView;
        view.InitSlots(40);
    }
    
    public void InitModel(List<SlotData<int>> datas)
    {
        model.InitModel(datas);
    }
    
    public void UpdateView(){
        if(!view.gameObject.activeSelf){
            pendingUpdates.Clear();
            foreach(var item in model.GetItemList()){
                pendingUpdates.Add(new SlotData<int>(item.Key, item.Value, item.Value.count));
            }
        }else{
            view.UpdateView(model.GetItemList());
        }
    }

    public void UpdateModel(SlotData<int> slot){
        model.UpdateModel(slot);
    } 

    public void AddItem(ItemData itemData)
    {
        model.AddItem(itemData);
    }

    public void ToggleInventory(){
        WindowController window = view.GetComponentInParent<WindowController>();

        bool isActive = !window.gameObject.activeSelf;
        window.gameObject.SetActive(isActive);
        
        if(isActive){
            foreach (var update in pendingUpdates) model.UpdateModel(update);
            
            pendingUpdates.Clear(); 
            view.UpdateView(model.GetItemList());
        }
    }

    public Item GetItemInstance(ItemData itemData){
        Item item = model.FindExistingItem(itemData);
        if(item == null) return null;
        return item;
    }
    #region Inspector Caller
    public Dictionary<int, Item> GetList() => model.GetItemList();
    #endregion
}