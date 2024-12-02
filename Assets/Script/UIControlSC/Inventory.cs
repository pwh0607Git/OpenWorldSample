using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory myInventory { get; private set; }

    private void Awake()
    {
        if (myInventory == null)
        {
            myInventory = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CreateInventorySlot();
    }

    public GameObject slotPrefab;
    public Transform scrollContent;                 //inventorySlot의 부모객체
    public List<InventorySlot> slots;               //Dictionary<InventorySlot, int>... 로 변경 예정.
    public int maxSlotSize;

    //슬롯에 있는 아이템 개수...
    private Dictionary<InventorySlot, int> slotItemCount = new Dictionary<InventorySlot, int>();

    void CreateInventorySlot()
    {
        int columns = 4;
        float spacingX = 0f;
        float spacingY = 0f;

        Vector2 startPosition = new Vector2(-120f, -50f);
        Vector2 componentSize = new Vector2(80f, 80f);

        for (int i = 0; i < maxSlotSize; i++)
        {
            GameObject slotInstance = Instantiate(slotPrefab);
            slotInstance.transform.SetParent(scrollContent);

            AddInventorySlotRef(slotInstance.GetComponent<InventorySlot>());

            int row = i / columns;
            int column = i % columns;

            RectTransform rectTransform = slotInstance.GetComponent<RectTransform>();
            rectTransform.sizeDelta = componentSize;
            rectTransform.anchoredPosition = new Vector2(startPosition.x + column * (componentSize.x + spacingX), startPosition.y - row * (componentSize.y + spacingY));
        }
    }

    private void AddInventorySlotRef(InventorySlot slotRef)
    {
        slots.Add(slotRef);
    }

    //초기화 완료 체크용 
    public bool CheckSlotSize()
    {
        //이후 코루틴을 통해.. 체크할 예정.
        return maxSlotSize == slots.Count;
    }

    public InventorySlot GetEmptyInventorySlot()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if (slots[i].transform.childCount == 0)
            {
                return slots[i];
            }
        }
        //빈 곳을 찾지 못한 경우...
        return null;
    }

    //ConsumeType? consumeType = null [Nullable 매개변수!!] 함수 호출시 해당 메서드는 강제 되지 않는다...
    //해당 아이템이 inventory에 존재하는지...
    public bool SearchItemByType(ItemType itemType, ConsumeType? consumeType = null)
    {
        foreach (InventorySlot slot in slots)
        {
            //비어있으면 넘어가기.
            if (slot.GetCurrentItem() == null)
                continue;

            ItemData itemData = slot.GetCurrentItem().GetComponent<ItemData>();

            if (itemData != null && itemData.itemType == itemType)
            {
                // Consumable 타입일 경우 potionType 확인
                if (itemType == ItemType.Consumable && consumeType.HasValue)
                {
                    Consumable consumable = itemData as Consumable;
                    if (consumable != null && consumable.potionType == consumeType.Value)
                    {
                        return true; // 해당 Consumable 찾음
                    }
                }
                else if (itemType == ItemType.Equipment)
                {
                    return true; // Equipment 찾음
                }
                else if (itemType == ItemType.ETC)
                {

                }
            }
        }
        return false; // 해당 아이템을 찾지 못함
    }
}