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

    public void UpdateView(Dictionary<EquipmentType, ItemData> slotDatas){
        foreach(var data in slotDatas){
            EquipmentSlot slot = CreateSlot(data.Key);
            if(data.Value == null) continue;

            // SetItemIcon(data.Value, slot);
        }
        
        UpdateViewInspector(slotDatas);
        EnableSlotEvents();
    }

    private void UpdateViewInspector(Dictionary<EquipmentType, ItemData> datas){
        inspectorView.Clear();
        foreach(var data in datas){
            if(data.Value == null) continue;
            inspectorView.Add(new SlotData<EquipmentType>(data.Key, data.Value));
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
        Debug.Log($"Equipment View Update : {data.slotKey} : {data.item}");
        foreach( var slot in slots){
            if(slot.GetItem() == null) continue;
            ItemData slotItem = slot.GetItem().GetComponent<ItemDataHandler>().GetItem;
            EquipmentType type = slot.type;

            SlotData<EquipmentType> viewData = new SlotData<EquipmentType>(type, slotItem);
            inspectorView.Add(viewData);
        }

        OnViewUpdated?.Invoke(data);
    }
}