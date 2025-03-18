using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using System;

public class EquipmentView : MonoBehaviour
{
    
    [Space(10)]
    [Header("UI Component")]
    Transform originalParent;

    [Space(10)]
    [Header("Prefabs")]
    [SerializeField] GameObject slotPrefab;
    [SerializeField] GameObject iconBasePrefab;
    public List<EquipmentSlot> slots = new List<EquipmentSlot>();
    public Dictionary<EquipmentType, ItemData> slotDictionary = new Dictionary<EquipmentType, ItemData>();

    [HorizontalLine("CurrentInventory"), HideField] public bool l1;
    [SerializeField, ReadOnly] List<SlotData<EquipmentType>> inspectorView;                              // 인스펙터 출력용
    [HorizontalLine(""), HideField] public bool l2;

    public event Action<SlotData<EquipmentType>> OnViewUpdated;  

    void Start(){
        originalParent = transform.parent;
        InitSlot();
    }

    public void InitSlot(){
        foreach(EquipmentType type in Enum.GetValues(typeof(EquipmentType))){
            CreateSlot(type);
        }
    }

    public void UpdateView(Dictionary<EquipmentType, Item> slotDatas){
        foreach(var data in slotDatas){
            EquipmentSlot slot = slots.Find(s => s.type == data.Key);
            if(data.Value == null) continue;

            SetItemIcon(data.Value, slot);
        }
        
        UpdateViewInspector(slotDatas);
    }

    EquipmentSlot CreateSlot(EquipmentType type){
        EquipmentSlot slot = Instantiate(slotPrefab, transform).GetComponent<EquipmentSlot>();
        slot.InitSlotDate(type, ChagedEventHandler);
        slots.Add(slot);
        return slot;
    }

    private void SetItemIcon(Item item, EquipmentSlot slot){
        ItemIcon itemIcon = UIIconFactory.Instance.CreateItemIcon(item);
        slot.SetItem(itemIcon.gameObject);
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
    
    private void UpdateViewInspector(Dictionary<EquipmentType, Item> slotDatas){
        inspectorView.Clear();
        foreach(var data in slotDatas){
            if(data.Value == null) continue;
            inspectorView.Add(new SlotData<EquipmentType>(data.Key, data.Value.data));
        }
    }
}