using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using CustomInspector;

public class InventoryView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Space(10)]
    [Header("UI Component")]
    [SerializeField] Transform scrollContent;
    public GameObject inventoryWindow;
    
    [Header("Prefabs")]
    [SerializeField] GameObject slotPrefab;
    [SerializeField] GameObject iconBasePrefab;
    private List<InventorySlot> slots = new List<InventorySlot>();
    Transform originalParent;

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
            slots.Add(slot);
        }
    }

    public void EnableSlotEvents()
    {
        foreach (var slot in slots)
        {
            slot.OnSlotUpdated += ChagedEventHandler;
        }
    }

    //==============================================================================//
    // ******************************************************************************

    // 인스턴스에서 count를 가지고 있다!!!!!!!!!!!!!!!!!!!
    
    // ******************************************************************************    
    //==============================================================================//
    
    public void UpdateView(Dictionary<int, Item> items){
        ClearSlotData();
        UpdateViewInspector(items);

        foreach(var data in itemsView){
            if(data.itemData == null) continue;
            SetItemIcon(ItemFactory.CreateItem(data.itemData, data.count), slots[data.slotKey]);
        }
    }

    private void UpdateViewInspector(Dictionary<int, Item> items){
        itemsView.Clear();
        foreach(var item in items){
            // if(item.Value == null) continue;
            itemsView.Add(new SlotData<int>(item.Key, item.Value.data));
        }
    }

    private void ClearSlotData(){
        foreach(var slot in slots){
            slot.ClearSlot();
        }
    }

    private void SetItemIcon(Item item, InventorySlot slot){
        ItemIcon itemIcon = Instantiate(iconBasePrefab, slot.transform).GetComponentInChildren<ItemIcon>();
        slot.SetItem(itemIcon.gameObject);
        AssignComponent(itemIcon, item);
    }

    private void AssignComponent(ItemIcon icon, Item item){
        icon.Initialize(item);
    }
    
    public void ChagedEventHandler(SlotData<int> data){
        StartCoroutine(Coroutine_ChangedEventHandle(data));
    }

    IEnumerator Coroutine_ChangedEventHandle(SlotData<int> data){
        yield return null;
        itemsView.Clear();
        for(int i=0;i<slots.Count;i++){
            if(slots[i].GetItem() == null) continue;
            Item slotItem = slots[i].GetItem().GetComponent<ItemIcon>().item;
            SlotData<int> viewData = new SlotData<int>(i, slotItem.data, slotItem.count);
            itemsView.Add(viewData);
        }

        OnViewUpdated?.Invoke(data);
    }

    #region Event
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        GetComponent<RectTransform>().SetParent(transform.root);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true; 
        GetComponent<RectTransform>().SetParent(originalParent);
    }
    #endregion
}