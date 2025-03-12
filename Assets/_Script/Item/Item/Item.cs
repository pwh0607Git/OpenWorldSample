using System;

[Serializable]
public abstract class Item
{
    public ItemData data{get;}
    
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
    public Consumable(ConsumableData data, int count = 1) : base(data, count) { }
    public event Action OnConsumableUsed;
    public void Use()
    {
        if (count <= 0) return;
        count--;

        ItemUsedManager.Instance.UseItem(this);
    }

    public void GetThisItem(){
        count++;
    }
}

[Serializable]
public class Equipment : Item {
    public Equipment(EquipmentData data, int count = 1) :base(data,count){ }
}