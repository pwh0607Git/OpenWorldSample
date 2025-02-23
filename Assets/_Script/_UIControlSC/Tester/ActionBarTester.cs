using System;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class ActionBarTester : MonoBehaviour
{
    public List<ActionBarSlotComponent> components = new List<ActionBarSlotComponent>();
    public PlayerUIPresenter uiPresenter;
    
    
    [Button("SendCodes"), HideField] public bool btn1;
    public void SendCodes(){
        uiPresenter.SerializeActionbar(components);
    }
}

[Serializable]
public class ActionBarSlotComponent{
    public KeyCode assignedKey;
    public ItemData assignedItem;
    public ActionBarSlotComponent(KeyCode key){
        assignedKey = key;
        assignedItem = null;
    }
}