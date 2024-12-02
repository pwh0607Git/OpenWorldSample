using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemMaker : MonoBehaviour
{
    [SerializeField]
    private List<Consumable> consumableItemList;
    [SerializeField]
    public GameObject iconPrefab;

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
            consumableItem.GetItem.Use();  // 아이템 사용
        }
        yield return null;
    }

    // 아이템을 생성하고 Consumable 데이터를 연결하는 함수
    public ConsumableItemSC MakeItem(Consumable itemData)
    {
        // 아이템 프리팹을 인스턴스화하고, ConsumableItemSC 스크립트를 가져옴
        GameObject item = Instantiate(iconPrefab);

        //인벤토리에서 빈 공간을 가져와 부모로 세팅하기.
        item.transform.SetParent(Inventory.myInventory.GetEmptyInventorySlot().gameObject.transform);
        ConsumableItemSC sc = item.GetComponent<ConsumableItemSC>();
        sc.GetItem = itemData;  // Consumable 데이터 연결
        return sc;
    }
}