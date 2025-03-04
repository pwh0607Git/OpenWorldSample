using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class ActionBarTester : MonoBehaviour
{
    public List<SlotData<KeyCode>> components = new List<SlotData<KeyCode>>();
    public PlayerUIPresenter uiPresenter;

    void Start()
    {
        StartCoroutine(SendCodes());
    }
    [Button("SendCodes"), HideField] public bool btn1;
    IEnumerator SendCodes(){
        yield return null;
        uiPresenter.InitActionbar(components);
    }
}