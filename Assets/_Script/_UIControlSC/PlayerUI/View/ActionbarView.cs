using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionbarView : MonoBehaviour
{
    public Transform slotParent;

    [Header("Prefabs")]
    [SerializeField] GameObject slotPrefab;
    [SerializeField] GameObject iconBasePrefab;

    public List<ActionBarSlotComponent> currentSlotList = new List<ActionBarSlotComponent>();
    public List<ActionBarSlot> viewSlots = new List<ActionBarSlot>();
    public void SerializeSlots(List<ActionBarSlotComponent> components){
        if(components == null) return;
        StartCoroutine(CoroutineSetSlots(components));
    }

    IEnumerator CoroutineSetSlots(List<ActionBarSlotComponent> components){
        yield return null;
            for(int i=0;i<components.Count;i++){
            ActionBarSlot slot = Instantiate(slotPrefab, slotParent).GetComponentInChildren<ActionBarSlot>();
            slot.SetAssigneKey(components[i].assignedKey);
            if(components[i].assignedItem == null) continue;
            
            //components[i].assignedItem = ItemData 이 데이터를 게임 오브젝트로 먼저 생성할 필요가 있음.
            // 1. 아이템 데이터 추출후, 오브젝트 생성
            ItemData itemData = components[i].assignedItem;

            GameObject itemIcon = Instantiate(iconBasePrefab, slot.transform);
            slot.AssignCurrentItem(itemIcon);
        }
    }
}
