using System.Collections.Generic;
using UnityEngine;

public class ActionbarView : MonoBehaviour
{
    public Transform slotParent;
    public GameObject slotPrefab;
    public List<ActionBarSlotComponent> slots = new List<ActionBarSlotComponent>();

    public void CreateSlotView(ActionBarSlotComponent slotModel){
        GameObject slot = Instantiate(slotPrefab, slotParent);
    }
}

public class ActionBarSlotComponent{
    public KeyCode assignedKey{get; private set;}
    public GameObject currentItem{get; set;}
    public ActionBarSlotComponent(KeyCode key){
        assignedKey = key;
    }
}