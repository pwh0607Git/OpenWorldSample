using System;

[Serializable]
public abstract class Item
{
    public virtual ItemData data{get;}
    
    public int count { get; protected set; }

    protected Item(ItemData data, int count = 1)
    {
        this.data = data;
        this.count = count;
    }
}

[Serializable]
public class Consumable : Item
{
    public override ItemData data => data as ConsumableData;
    public Consumable(ConsumableData data, int count = 1) : base(data, count) { }
    public void Use()
    {
        if (count <= 0) return;
        count--;

        if(data is BuffConsumableData){
            ItemUsedManager.Instance.UseBuffItem(this);
            return;
        }
        ItemUsedManager.Instance.UseItem(this);
    }

    public void GetThisItem(){
        count++;
    }
}

[Serializable]
public class Equipment : Item {
    public override ItemData data => data as EquipmentData;
    public Equipment(EquipmentData data, int count = 1) :base(data,count){ }
}