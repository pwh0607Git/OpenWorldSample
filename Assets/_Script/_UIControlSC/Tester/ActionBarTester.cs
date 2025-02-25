using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class ActionBarTester : MonoBehaviour
{
    public List<ActionBarSlotComponent> components = new List<ActionBarSlotComponent>();
    public PlayerUIPresenter uiPresenter;

    void Start()
    {
        StartCoroutine(SendCodes());
    }
    [Button("SendCodes"), HideField] public bool btn1;
    IEnumerator SendCodes(){
        yield return null;
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