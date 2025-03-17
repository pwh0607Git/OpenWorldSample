using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using CustomInspector;

public class InventoryView : MonoBehaviour
{
    [Space(10)]
    [Header("UI Component")]
    [SerializeField] Transform scrollContent;
    public GameObject inventoryWindow;

    Transform originalParent;
    
    [Space(10)]
    [Header("Prefabs")]
    [SerializeField] GameObject slotPrefab;
    [SerializeField] GameObject iconBasePrefab;
    private List<InventorySlot> slots = new List<InventorySlot>();

    [HorizontalLine("CurrentInventory"), HideField] public bool l1;
    [SerializeField, ReadOnly] List<SlotData<int>> itemsView;                              // 인스펙터 출력용
    [HorizontalLine(""), HideField] public bool l2;

    public event Action<SlotData<int>> OnViewUpdated;          //inventoryView의 변화 감지 
    void Start(){
        originalParent = transform.parent;
    }

    public void SetActive(bool isActive){
        inventoryWindow.SetActive(isActive);
    }
    
    public void InitSlots(int maxSlotSize){
        for(int i=0;i<maxSlotSize;i++){
            InventorySlot slot = Instantiate(slotPrefab, scrollContent).GetComponent<InventorySlot>();
            slot.index = i;
            slot.OnSlotUpdated += ChagedEventHandler;
            slots.Add(slot);
        }
    }

    public void UpdateView(Dictionary<int, Item> items){
        ClearSlotData();
        Debug.Log($"Inventory View : Update View!");
        // UpdateViewInspector(items);

        foreach(var item in items){
            if(item.Value == null) continue;
            SetItemIcon(item.Value, slots[item.Key]);
        }
    }

    private void ClearSlotData(){
        foreach(var slot in slots){
            slot.ClearSlot();
        }
    }

    IEnumerator Coroutine_ChangedEventHandle(SlotData<int> data){
        yield return null;
        OnViewUpdated?.Invoke(data);

        //인스펙터 갱신
        itemsView.Clear();
        for(int i=0;i<slots.Count;i++){
            if(slots[i].GetItem() == null) continue;
            Item slotItem = slots[i].GetItem().GetComponent<ItemIcon>().item;
            
            SlotData<int> viewData = new SlotData<int>(i, slotItem.data, slotItem.count);
            itemsView.Add(viewData);
        }
    }
    private void SetItemIcon(Item item, InventorySlot slot){
        ItemIcon itemIcon = UIIconFactory.Instance.CreateItemIcon(item);
        slot.SetItem(itemIcon.gameObject);
    }
    
    public void ChagedEventHandler(SlotData<int> data){
        StartCoroutine(Coroutine_ChangedEventHandle(data));
    }

    #region Inspector View
        
    private void UpdateViewInspector(Dictionary<int, Item> items){
        itemsView.Clear();
        foreach(var item in items){
            if(item.Value == null) continue;
            itemsView.Add(new SlotData<int>(item.Key, item.Value.data));
        }
    }
    #endregion
}