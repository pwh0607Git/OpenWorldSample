using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryItemIconManager : MonoBehaviour
{
    [SerializeField]
    private List<Consumable> consumableItemList;            //테스트용 코드...

    [SerializeField]
    public GameObject iconBasePrefab;           //icon Base..

    public Transform scrollContent;

    public void CreateItemIcon(ItemData newItemData, InventorySlot emptySlot)
    {
        StartCoroutine(CreateItemIconCoroutine(newItemData, emptySlot));
    }

    IEnumerator CreateItemIconCoroutine(ItemData newItemData, InventorySlot emptySlot)
    {
        while (!Inventory.myInventory.CheckSlotSize())
        {
            yield return null;
        }

        var newItemIcon = AssignItemData(newItemData, emptySlot);
        newItemIcon.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        //인벤토리에서 빈 공간을 가져와 부모로 세팅하기.
        newItemIcon.gameObject.transform.SetParent(emptySlot.gameObject.transform);
        newItemIcon.GetComponent<RectTransform>().localScale = Vector2.one;             //칸 중앙 정렬.

        Inventory.myInventory.UpdateInventorySlots();
        yield return null;
    }

    // 아이템을 생성하고 해당 데이터에 걸맞는 SC를 연결하는 함수
    public ItemDataSC AssignItemData(ItemData itemData, InventorySlot emptySlot)
    {
        GameObject item = Instantiate(iconBasePrefab);

        ItemDataSC itemDataSC = item.GetComponent<ItemDataSC>();
        if (itemDataSC != null)
        {
            if (itemData.itemType == ItemType.Consumable && itemData is Consumable consumable)
            {
                ((ConsumableItemSC)itemDataSC).SetItem(consumable);             // Consumable 초기화
            }
            else if (itemData.itemType == ItemType.Equipment && itemData is Equipment equipment)
            {
                //((EquipmentItemSC)itemDataSC).SetItem(equipment);             // Equipment 초기화
            }
            else
            {
                Debug.LogError("알 수 없는 아이템 타입입니다.");
            }
        }
        return itemDataSC;
    }
}