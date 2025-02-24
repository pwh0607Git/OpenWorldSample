using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using CustomInspector.Extensions;
using System.Threading;
using Unity.VisualScripting;
public class InventoryItemIconManager : MonoBehaviour
{
    [SerializeField]
    private List<Consumable> consumableItemList;

    [SerializeField]
    public GameObject iconBasePrefab;

    public Transform scrollContent;

    public void CreateItemIcon(ItemData newItemData, InventorySlot emptySlot)
    {
        StartCoroutine(CreateItemIconCoroutine(newItemData, emptySlot));
    }

    IEnumerator CreateItemIconCoroutine(ItemData newItemData, InventorySlot emptySlot)
    {
        yield return new WaitUntil(()=>PlayerController.uiController.CheckSlotSize() == true);
        while (!PlayerController.uiController.CheckSlotSize())
        {
            yield return null;
        }

        var newItemIcon = AssignItemData(newItemData, emptySlot);
        newItemIcon.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        newItemIcon.gameObject.transform.SetParent(emptySlot.gameObject.transform);
        newItemIcon.GetComponent<RectTransform>().localScale = Vector2.one;

        emptySlot.AssignCurrentItem(newItemIcon.gameObject);

        PlayerController.uiController.SyncUIData();
        yield return null;
    }

    public ItemDataHandler AssignItemData(ItemData itemData, InventorySlot emptySlot)
    {
        if (itemData == null) return null;

        GameObject itemIcon = Instantiate(iconBasePrefab, emptySlot.transform);
        ItemDataHandler itemDataHandler;
        
        if (itemData.itemType == ItemType.Consumable)
        {
            ConsumableItemHandler consumableHandler = itemIcon.AddComponent<ConsumableItemHandler>();
            consumableHandler.Init(itemData);
        }else if(itemData.itemType == ItemType.Equipment)
        {
            EquipmentItemHandler equipmentHandler = itemIcon.AddComponent<EquipmentItemHandler>();
            equipmentHandler.Init(itemData);
        }

        itemDataHandler = itemIcon.GetComponentInChildren<ItemDataHandler>();
        return itemDataHandler;
    }
}