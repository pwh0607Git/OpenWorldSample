using System;
using System.Collections.Generic;
using UnityEngine;
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

    public void UpdateView(Dictionary<KeyCode, Item> slotDatas){
        foreach(var data in slotDatas){
            // 해당 슬롯은 KeyCode 데이터만 가지고 있다.
            ActionBarSlot slot = CreateSlot(data.Key);
            if(data.Value == null) continue;
            SetItemIcon(data.Value, slot);
        }
        // UpdateViewInspector(slotDatas);
    }

    private void ClearSlotData(){
        foreach(var slot in slots){
            slot.ClearSlot();
        }
    }

    ActionBarSlot CreateSlot(KeyCode key){
        ActionBarSlot slot = Instantiate(slotPrefab, slotParent).GetComponent<ActionBarSlot>();
        slots.Add(slot);
        slot.assignedKey = key;
        slot.OnSlotUpdated += ChagedEventHandler;
        return slot;
    }

    public void ChagedEventHandler(SlotData<KeyCode> data){
        StartCoroutine(Coroutine_ChangedEventHandle(data));
    }

    // 변경된 데이터
    IEnumerator Coroutine_ChangedEventHandle(SlotData<KeyCode> data){
        yield return null;
        OnViewUpdated?.Invoke(data);

        inspectorView.Clear();
        Debug.Log($"Actionbar Veiw Update : {data.slotKey} : {data.itemData}");
        foreach(var slot in slots){
            if(slot.GetItem() == null) continue;
            Item slotItem = slot.GetItem().GetComponent<ItemIcon>().item;
            KeyCode key = slot.assignedKey;

            SlotData<KeyCode> viewData = new SlotData<KeyCode>(key, slotItem.data);
            inspectorView.Add(viewData);
        }
    }

    private void SetItemIcon(Item item, ActionBarSlot slot){
        ItemIcon itemIcon = UIIconFactory.Instance.CreateItemIcon(item);
        slot.SetItem(itemIcon.gameObject);
    }

    #region Inspector View
    private void UpdateViewInspector(Dictionary<KeyCode, Item> datas){
        inspectorView.Clear();
        foreach(var data in datas){
            if(data.Value == null){
                inspectorView.Add(new SlotData<KeyCode>(data.Key));    
            }
            else{
                inspectorView.Add(new SlotData<KeyCode>(data.Key, data.Value.data, data.Value.count));
            }
        }
    }
    #endregion
}
