using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

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
        while (!PlayerController.myInventory.CheckSlotSize())
        {
            yield return null;
        }

        var newItemIcon = AssignItemData(newItemData, emptySlot);
        newItemIcon.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        newItemIcon.gameObject.transform.SetParent(emptySlot.gameObject.transform);
        newItemIcon.GetComponent<RectTransform>().localScale = Vector2.one;

        emptySlot.AssignCurrentItem(newItemIcon.gameObject);

        PlayerController.myInventory.SyncUIData();
        yield return null;
    }

    public ItemDataSC AssignItemData(ItemData itemData, InventorySlot emptySlot)
    {
        if (itemData == null) return null;

        GameObject item = Instantiate(iconBasePrefab);

        if (itemData.itemType == ItemType.Consumable)
        {
            item.AddComponent<ConsumableItemSC>();
        }else if(itemData.itemType == ItemType.Equipment)
        {
            item.AddComponent<EquipmentItemSC>();
        }
        
        ItemDataSC itemDataSC = item.GetComponent<ItemDataSC>();
        if (itemDataSC != null)
        {
            if (itemData.itemType == ItemType.Consumable && itemData is Consumable consumable)
            {
                ((ConsumableItemSC)itemDataSC).SetItem(consumable);
            }
            else if (itemData.itemType == ItemType.Equipment && itemData is Equipment equipment)
            {
                Destroy(item.transform.GetChild(0).gameObject);     //Text 삭제.
                ((EquipmentItemSC)itemDataSC).SetItem(equipment);
            }
            else
            {
                Debug.LogError("알 수 없는 아이템 타입입니다.");
            }
        }
        return itemDataSC;
    }
}