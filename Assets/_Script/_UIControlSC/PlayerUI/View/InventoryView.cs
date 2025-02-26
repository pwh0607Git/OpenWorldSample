using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using CustomInspector;

public class InventoryView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField, ReadOnly] List<ItemEntry> itemsView;                              // 인스펙터 출력용
    [Header("UI Component")]
    [SerializeField] Transform scrollContent;
    public GameObject inventoryWindow;
    
    [Header("Prefabs")]
    [SerializeField] GameObject slotPrefab;
    [SerializeField] GameObject iconBasePrefab;
    private List<InventorySlot> slots = new List<InventorySlot>();
    Transform originalParent;

    public event Action<List<ItemEntry>> OnChangedInventoryView;          //inventoryView의 변화 감지 
    void Start()
    {
        originalParent = transform.parent;
    }
    public void SetActive(bool isActive)
    {
        inventoryWindow.SetActive(isActive);
    }
    
    public void CreateSlots(int maxSlotSize){
        for(int i=0;i<maxSlotSize;i++){
            InventorySlot slot = Instantiate(slotPrefab, scrollContent).GetComponent<InventorySlot>();
            slots.Add(slot);
        }
    }

    //index : ItemData
    public void UpdateView(List<ItemEntry> items){
        Debug.Log($"Inventory View : 받은 List Count : {items.Count}");
        // 엔트리 리스트에 존재하는 아이템에 대해서만 icon을 생성하고 나머지 아이콘들은 모두 제거한다.
        ClearSlotData();
        this.itemsView = items;

        // item : Entry
        foreach(var item in itemsView){
            if(item.indexItem == null) continue;
            SetItemIcon(item.indexItem,slots[item.inventoryIdx]);
        }
    }

    private void ClearSlotData(){
        foreach(var slot in slots){
            slot.ClearCurrentItem();
        }
    }

    private void SetItemIcon(ItemData item, InventorySlot slot){
        GameObject itemIcon = Instantiate(iconBasePrefab, slot.transform);
        slot.AssignCurrentItem(itemIcon);
        AssignComponent(itemIcon,item);
    }

    private void AssignComponent(GameObject icon, ItemData itemData){
        ItemDataHandler handler = null;
        if (itemData.itemType == ItemType.Consumable)
        {
            handler = icon.AddComponent<ConsumableItemHandler>();
        }
        else if(itemData.itemType == ItemType.Equipment)
        {
            handler =  icon.AddComponent<EquipmentItemHandler>();
        }

        if(handler == null) return;
        handler.Init(itemData);
    }
    public void ChagedEventHandler(){
        //View로 부터 데이터 변화가 발생!
        StartCoroutine(Coroutine_ChangedEventHandle());
    }

    IEnumerator Coroutine_ChangedEventHandle(){
        yield return null;
        Debug.Log($"Inventory View : Change Event 발생!");
        
        //슬롯 전체를 비교.
        itemsView.Clear();
        for(int i=0;i<slots.Count;i++){
            if(slots[i].currentItem == null) continue;
            ItemData slotItem = slots[i].currentItem.GetComponent<ItemDataHandler>().GetItem;
            ItemEntry entry = new ItemEntry(i, slotItem);
            itemsView.Add(entry);
        }
        OnChangedInventoryView?.Invoke(itemsView);
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