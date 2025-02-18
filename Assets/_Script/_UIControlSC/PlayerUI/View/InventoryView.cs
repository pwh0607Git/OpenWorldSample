using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class InventoryView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject inventoryWindow;
    [SerializeField] Transform scrollContent;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] GameObject iconBasePrefab;
    
    private List<InventorySlot> slots = new List<InventorySlot>();
    [SerializeField] List<ItemData> items;
    Transform originalParent;

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

    public void UpdateView(Dictionary<int, ItemData> items){
        foreach(var item in items){
            if(item.Value != null) this.items.Add(item.Value);
        } 

        for(int i=0;i<items.Count; i++){
            if(items[i] != null)
                MakeItemIcon(items[i], slots[i]);
        }  
        
    }

    //인덱스에 걸맞는 슬롯에 아이템 추가하기.
    public void MakeItemIcon(ItemData item, InventorySlot slot){
        GameObject itemIcon = Instantiate(iconBasePrefab, slot.transform);
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        GetComponent<RectTransform>().SetParent(transform.root);                // �������� �ֻ����� �̵� (canvas)
        GetComponent<CanvasGroup>().blocksRaycasts = false;                     // �巡�� �� ����� �������� ����
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
}
