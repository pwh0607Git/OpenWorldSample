using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemMaker : MonoBehaviour
{
    [SerializeField]
    private List<Consumable> consumableItemList;

    [SerializeField]
    public GameObject iconBasePrefab;

    public Transform scrollContent;

    private void Start()
    {
        StartCoroutine(MakeItems());
    }

    IEnumerator MakeItems()
    {
        while (!Inventory.myInventory.CheckSlotSize())
        {
            yield return null;
        }

        foreach (var item in consumableItemList)
        {
            // 아이템을 생성하고, 아이템 데이터를 연결
            var consumableItem = MakeItem(item);
            consumableItem.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        Inventory.myInventory.SyncUIData();
        yield return null;
    }

    // 아이템을 생성하고 해당 데이터에 걸맞는 SC를 연결하는 함수
    public ItemDataSC MakeItem(ItemData itemData)
    {
        GameObject item = Instantiate(iconBasePrefab);

        //인벤토리에서 빈 공간을 가져와 부모로 세팅하기.
        item.transform.SetParent(Inventory.myInventory.GetEmptyInventorySlot().gameObject.transform);

        item.GetComponent<RectTransform>().localScale = Vector2.one;        //칸 중앙 정렬.

        ItemDataSC itemDataSC = item.GetComponent<ItemDataSC>();

        if(itemDataSC != null)
        {
            if (itemData.itemType == ItemType.Consumable && itemData is Consumable consumable)
            {
                ((ConsumableItemSC)itemDataSC).SetItem(consumable);         // Consumable 초기화
            }else if(itemData.itemType == ItemType.Equipment && itemData is Equipment equipment)
            {
                //((EquipmentItemSC)itemDataSC).SetItem(equipment);         // Consumable 초기화
            }
            else
            {
                Debug.LogError("알 수 없는 아이템 타입입니다.");
            }
        }
        return itemDataSC;
    }
}