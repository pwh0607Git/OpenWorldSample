using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class InventoryView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] List<ItemEntry> items;                              // 인스펙터 출력용
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
            slot.OnChangedItem += ChagedEventHandler;
            slots.Add(slot);
        }
    }

    //index : ItemData
    public void UpdateView(Dictionary<int, ItemData> items){
        foreach(var item in items){
            if(item.Value != null) this.items.Add(new ItemEntry(item.Key,item.Value,2));
        } 

        for(int i=0;i<items.Count; i++){
            if(items[i] != null)
                MakeItemIcon(items[i], slots[i]);
            else{
                slots[i].ClearCurrentItem();
            }
        }  
        
    }

    public void MakeItemIcon(ItemData item, InventorySlot slot){
        GameObject itemIcon = Instantiate(iconBasePrefab, slot.transform);
        slot.AssignCurrentItem(itemIcon);
        AssignComponent(itemIcon,item);

    }

    private void AssignComponent(GameObject icon, ItemData item){
        if (item.itemType == ItemType.Consumable)
        {
            icon.AddComponent<ConsumableItemSC>();
        }else if(item.itemType == ItemType.Equipment)
        {
            icon.AddComponent<EquipmentItemSC>();
        }

        ItemDataSC itemDataSC = icon.GetComponent<ItemDataSC>();
        if (itemDataSC != null)
        {
            if (item.itemType == ItemType.Consumable && item is Consumable consumable)
            {
                ((ConsumableItemSC)itemDataSC).SetItem(consumable);
            }
            else if (item.itemType == ItemType.Equipment && item is Equipment equipment)
            {
                Destroy(icon.GetComponentInChildren<TextMeshProUGUI>().gameObject);
                ((EquipmentItemSC)itemDataSC).SetItem(equipment);
            }
        }
    }
    public void ChagedEventHandler(){
        //View로 부터 데이터 변화가 발생!
        StartCoroutine(Coroutine_ChangedEventHandle());
    }

    IEnumerator Coroutine_ChangedEventHandle(){
        yield return null;
        items.Clear();
        for(int i=0;i<slots.Count;i++){
            if(slots[i].currentItem == null) continue;
            ItemEntry entry = new ItemEntry(i, slots[i].currentItem.GetComponent<ItemDataSC>().GetItem, 1);
            Debug.Log($"{entry.inventoryIdx} : {entry.indexItem}");
            items.Add(entry);
        }
        OnChangedInventoryView?.Invoke(items);
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