using UnityEngine;

public class UIIconFactory : BehaviourSingleton<UIIconFactory>
{
    [Header("Icon Prefab")]
    [SerializeField] private GameObject iconPrefab;
    protected override bool IsDontDestroy() => false;
    public ItemIcon CreateItemIcon(ItemData data, int count){
        ItemIcon icon = Instantiate(iconPrefab).GetComponent<ItemIcon>();
        icon.Initialize(ItemFactory.CreateItem(data, count));
        return icon;
    }

    public ItemIcon CreateItemIcon(Item item){
        ItemIcon icon = Instantiate(iconPrefab).GetComponent<ItemIcon>();
        icon.Initialize(item);
        
        return icon;
    }
}

public static class ItemFactory
{
    public static Item CreateItem(ItemData data, int count = 1)
    {
        return data switch
        {
            ConsumableData cd => new Consumable(cd, count),
            EquipmentData ed => new Equipment(ed, count),
            _ => null
        };
    }
}