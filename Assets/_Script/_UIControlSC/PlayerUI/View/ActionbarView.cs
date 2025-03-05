using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using CustomInspector;

public class ActionbarView : MonoBehaviour
{
    public Transform slotParent;

    [Header("Prefabs")]
    [SerializeField] GameObject slotPrefab;
    [SerializeField] GameObject iconBasePrefab;

    [Header("Datas")]
    public Dictionary<KeyCode, ItemData> slotDictionary = new Dictionary<KeyCode, ItemData>();
    public List<ActionBarSlot> slots = new List<ActionBarSlot>();
    
    [HorizontalLine("CurrentInventory"), HideField] public bool l1;
    [SerializeField, ReadOnly] List<SlotData<KeyCode>> inspectorView;                              // 인스펙터 출력용
    [HorizontalLine(""), HideField] public bool l2;
    
    public event Action<SlotData<KeyCode>> OnViewUpdated;  

    private void UpdateViewInspector(Dictionary<KeyCode, ItemData> datas){
        inspectorView.Clear();
        foreach(var data in datas){
            if(data.Value == null) continue;
            inspectorView.Add(new SlotData<KeyCode>(data.Key, data.Value));
        }
    }
    public void UpdateView(Dictionary<KeyCode, ItemData> slotDatas){
        foreach(var data in slotDatas){
            ActionBarSlot slot = CreateSlot(data.Key);
            if(data.Value == null) continue;

            SetItemIcon(data.Value, slot);
        }
        
        UpdateViewInspector(slotDatas);
        EnableSlotEvents();
    }

    ActionBarSlot CreateSlot(KeyCode key){
        ActionBarSlot slot = Instantiate(slotPrefab, slotParent).GetComponent<ActionBarSlot>();
        slots.Add(slot);
        slot.assignedKey = key;
        return slot;
    }

    public void EnableSlotEvents()
    {
        foreach (var slot in slots)
        {
            slot.OnSlotUpdated += ChagedEventHandler;
        }
    }

    public void ChagedEventHandler(SlotData<KeyCode> data){
        StartCoroutine(Coroutine_ChangedEventHandle(data));
    }

    // // 변경된 데이터
    IEnumerator Coroutine_ChangedEventHandle(SlotData<KeyCode> data){
        yield return null;
        inspectorView.Clear();
        Debug.Log($"Actionbar Veiw Update : {data.slotKey} : {data.item}");
        foreach( var slot in slots){
            if(slot.GetItem() == null) continue;
            ItemData slotItem = slot.GetItem().GetComponent<ItemDataHandler>().GetItem;
            KeyCode key = slot.assignedKey;

            SlotData<KeyCode> viewData = new SlotData<KeyCode>(key, slotItem);
            inspectorView.Add(viewData);
        }

        OnViewUpdated?.Invoke(data);
    }

    private void SetItemIcon(ItemData item, ActionBarSlot slot){
        GameObject itemIcon = Instantiate(iconBasePrefab, slot.transform);
        slot.SetItem(itemIcon);
        AssignComponent(itemIcon, item);
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
}
