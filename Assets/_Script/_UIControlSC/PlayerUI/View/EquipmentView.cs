using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using CustomInspector;

public class EquipmentView : MonoBehaviour
{
    public Transform slotParent;

    [Header("Prefabs")]
    [SerializeField] GameObject slotPrefab;
    [SerializeField] GameObject iconBasePrefab;
    public Dictionary<EquipmentType, ItemData> slotDictionary = new Dictionary<EquipmentType, ItemData>();
    public List<EquipmentSlot> slots = new List<EquipmentSlot>();

    [HorizontalLine("CurrentInventory"), HideField] public bool l1;
    [SerializeField, ReadOnly] List<SlotData<EquipmentType>> inspectorView;                              // 인스펙터 출력용
    [HorizontalLine(""), HideField] public bool l2;

    public event Action<SlotData<EquipmentType>> OnViewUpdated;  

    public void EnableSlotEvents()
    {
        foreach (var slot in slots)
        {
            slot.OnSlotUpdated += ChagedEventHandler;
        }
    }

    public void UpdateView(Dictionary<EquipmentType, Item> slotDatas){
        foreach(var data in slotDatas){
            EquipmentSlot slot = CreateSlot(data.Key);
            if(data.Value == null) continue;

            // SetItemIcon(data.Value, slot);
        }
        
        UpdateViewInspector(slotDatas);
        EnableSlotEvents();
    }

    private void UpdateViewInspector(Dictionary<EquipmentType, Item> slotDatas){
        inspectorView.Clear();
        foreach(var data in slotDatas){
            if(data.Value == null) continue;
            inspectorView.Add(new SlotData<EquipmentType>(data.Key, data.Value.data));
        }
    }

    EquipmentSlot CreateSlot(EquipmentType type){
        EquipmentSlot slot = Instantiate(slotPrefab, slotParent).GetComponent<EquipmentSlot>();
        slots.Add(slot);
        slot.type = type;
        return slot;
    }

    public void ChagedEventHandler(SlotData<EquipmentType> data){
        StartCoroutine(Coroutine_ChangedEventHandle(data));
    }

    // // 변경된 데이터
    IEnumerator Coroutine_ChangedEventHandle(SlotData<EquipmentType> data){
        yield return null;
        inspectorView.Clear();
        Debug.Log($"Equipment View Update : {data.slotKey} : {data.itemData}");
        foreach( var slot in slots){
            if(slot.GetItem() == null) continue;
            Item slotItem = slot.GetItem().GetComponent<ItemIcon>().item;
            EquipmentType type = slot.type;

            SlotData<EquipmentType> viewData = new SlotData<EquipmentType>(type, slotItem.data);
            inspectorView.Add(viewData);
        }

        OnViewUpdated?.Invoke(data);
    }
}