using UnityEngine;

public enum EquipmentType
{
    Head,                           
    Weapon,                         
    Cloth,                        
    Foot                           
}

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment")]
public class Equipment : ItemData, IDroppableItem
{
    public EquipmentType subType;

    public void Init()
    {

    }

    public ItemData GetItemData() => this;

    private void OnEnable()
    {
        itemType = ItemType.Equipment;
    }

    public override void Use(){
        State state = PlayerController.player.myState;
        PlayerController.player.SetEquipment(model);
        state.EquipItem(this);
    }

    public void Detach()
    {
        State state = PlayerController.player.myState;
        PlayerController.player.CleanEquipment();
        state.DetachItem(this);
    }
}