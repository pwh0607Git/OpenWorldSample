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