using UnityEngine;

public class UIIconFactory : MonoBehaviour
{
    [Header("Icon Prefab")]
    [SerializeField] private GameObject iconPrefab;

    public static UIIconFactory Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public ItemIcon CreateItemIcon(ItemData data, int count){
        ItemIcon icon = Instantiate(iconPrefab).GetComponent<ItemIcon>();
        icon.Initialize(ItemFactory.CreateItem(data, count));
        return icon;
    }

    public ItemIcon CreateItemIcon(Item item){
        ItemIcon icon = Instantiate(iconPrefab).GetComponent<ItemIcon>();
        icon.Initialize(item);
        if (item is Consumable consumable) consumable.SubscribeToUseEvent(icon.UpdateIcon);
        
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