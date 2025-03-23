using System;
using UnityEngine.Events;

[Serializable]
public abstract class Item
{
    public virtual ItemData data{get;}
    
    public int count { get; protected set; }

    public virtual event UnityAction OnItemUsed;

    protected Item(ItemData data, int count = 1)
    {
        this.data = data;
        this.count = count;
    }
}

[Serializable]
public class Consumable : Item
{
    public override ItemData data => base.data as ConsumableData;
    public override event UnityAction OnItemUsed;
    public Consumable(ConsumableData data, int count = 1) : base(data, count) { }
    public void Use()
    {
        if (count <= 0) return;
        count--;
        ItemUsedManager.Instance.UseItem(this);
        OnItemUsed?.Invoke();
    }

    public void GetThisItem(){
        count++;
    }
}

[Serializable]
public class Equipment : Item {
    public override ItemData data => base.data as EquipmentData;
    public Equipment(EquipmentData data, int count = 1) :base(data,count){ }
}