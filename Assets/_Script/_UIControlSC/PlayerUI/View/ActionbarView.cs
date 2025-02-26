using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionbarView : MonoBehaviour
{
    public Transform slotParent;

    [Header("Prefabs")]
    [SerializeField] GameObject slotPrefab;
    [SerializeField] GameObject iconBasePrefab;

    [Header("Datas")]
    public Dictionary<KeyCode, ItemData> slotDictionary = new Dictionary<KeyCode, ItemData>();
    public List<ActionBarSlot> viewSlots = new List<ActionBarSlot>();
    public void SerializeSlots(List<ActionBarSlotComponent> components){
        if(components == null) return;
        StartCoroutine(CoroutineSetSlots(components));
    }

    IEnumerator CoroutineSetSlots(List<ActionBarSlotComponent> components){
        yield return null;
        
        // 1. 빈슬롯에 키 값 할당
        foreach(var c in components){
            ActionBarSlot slot = Instantiate(slotPrefab, slotParent).GetComponentInChildren<ActionBarSlot>();
            slot.SetAssigneKey(c.assignedKey);
            slotDictionary[c.assignedKey] = null;
            slot.OnActionBarUpdated += ChagedEventHandler;
            
            if(c.assignedItem == null) continue;

            //아이콘 생성
            GameObject itemIcon = Instantiate(iconBasePrefab, slot.transform);
            slot.AssignCurrentItem(itemIcon);
            AssignComponent(itemIcon, c.assignedItem);
        }
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

    public void ChagedEventHandler(KeyCode key, GameObject item){
        ItemDataHandler itemDataHandler = item.GetComponentInChildren<ItemDataHandler>();
        if(itemDataHandler != null){
            slotDictionary[key] = itemDataHandler.GetItem;
        }else{
            //소비 아이템이 아닌경우...
        }

        
    }

    IEnumerator Coroutine_ChangedEventHandle(){
        yield return null;
        Debug.Log($"Actionbar View : Change Event 발생!");

    }
}
