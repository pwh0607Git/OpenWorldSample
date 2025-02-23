using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionbarModel 
{
    public int maxSlotSize {get; private set;}
    private List<ActionBarSlotComponent> components;
    public event Action OnModelChanged;             // inventory 내 아이템 정보가 갱신되면 실행되는 이벤트.

    public ActionbarModel()
    {
        this.maxSlotSize = maxSlotSize;
        components = new List<ActionBarSlotComponent>();
    }

    public List<ActionBarSlotComponent> GetComponents(){
        return new List<ActionBarSlotComponent>(components);
    }
    public void UpdateModel(List<ActionBarSlotComponent> components){
        Debug.Log($"Inventory Model : Serialize => {components.Count}");
        this.components = components;
    }
    
    public void Serialize(List<ActionBarSlotComponent> components){
        UpdateModel(components);
        OnModelChanged?.Invoke();
    }
}