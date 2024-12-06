using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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
        inventoryItemIconManager = GetComponent<InventoryItemIconManager>();
    }

    public GameObject slotPrefab;
    public Transform scrollContent;                 //inventorySlot의 부모객체
    public List<InventorySlot> slots;               //Dictionary<InventorySlot, int>... 로 변경 예정.
    public int maxSlotSize;

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
            rectTransform.localScale = Vector2.one;
            rectTransform.anchoredPosition = new Vector2(startPosition.x + column * (componentSize.x + spacingX), startPosition.y - row * (componentSize.y + spacingY));
        }
    }

    private void AddInventorySlotRef(InventorySlot slotRef)
    {
        slots.Add(slotRef);
    }

    public bool CheckSlotSize()
    {
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
        return null;
    }

    private InventoryItemIconManager inventoryItemIconManager; 

    //미완성..
    public bool SearchItemByType<T>(ItemType itemType, T? subType = null) where T : struct
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.GetCurrentItem() == null) continue;

            ItemData slotItemData = slot.GetCurrentItem().GetComponent<ItemDataSC>().GetItem;

            if (slot.GetCurrentItem() != null && slotItemData.itemType == itemType)
            {
                if (subType == null) return true;
                if (itemType == ItemType.Consumable && slotItemData is Consumable consumable)
                {
                    if (EqualityComparer<T>.Default.Equals((T)(object)consumable.subType, subType.Value))
                        return true;
                }
                else if (itemType == ItemType.Equipment && slotItemData is Equipment equipment)
                {
                    if (EqualityComparer<T>.Default.Equals((T)(object)equipment.subType, subType.Value))
                        return true;
                }
            }
        }
        return false;
    }
    
    public void GetItem(GameObject item)
    {
        ItemDataSC itemDataSC = item.GetComponent<ItemDataSC>();            //DroppedItemSC를 참조한다...
        ItemData itemData = itemDataSC.GetItem;         

        //아이템이 존재하지 않으면 아이템 아이콘 생성.
        //먹은 아이템 구분하기
        if (itemData != null) 
        {
            if (itemData is Consumable consumable)
            {
               if (SearchItemByType<ConsumableType>(itemData.itemType, consumable.subType)){
                    Debug.Log($"{consumable.subType}은 인벤토리에 존재...");
                    consumable.GetThisItem();
                    return;
               }
               else
               {
                    //새로운 아이콘 생성하고 빈 슬롯에 넣고 할당하기.
                    GetNewItem(itemData);
                }
            }
            else if (itemData is Equipment equipment)
            {
                if (SearchItemByType<EquipmentType>(itemData.itemType, equipment.subType))
                {
                    Debug.Log($"{equipment.subType}은 인벤토리에 존재...");
                    //아이콘 추가!.
                    //장비아이템은 소지 개수를 구분하지 않도록한다.
                    //그냥 무조건 아이템 아이콘을 빈 슬롯에 추가한다.
                    return;
                }
            }
        }
        else
        {
            Debug.Log("ItemData is Null...");
        }
    }

    //inventory에 존재하지 않은 아이템 GET
    public void GetNewItem(ItemData newItem)
    {
        InventorySlot emptySlot = GetEmptyInventorySlot();
        inventoryItemIconManager.CreateItemIcon(newItem, emptySlot);
    }

    private void HandleConsumableItem(ConsumableItemSC consumableItemSC)
    {
 
    }

    private void HandleEquipmentItem()
    {

    }

    public void UpdateInventorySlots()
    {
        Debug.Log("아이템 리스트 업데이트");
        foreach (InventorySlot slot in slots)
        {
            if (slot.transform.childCount == 0) continue;
            else
            {
                slot.AssignItem(slot.transform.GetChild(0).gameObject);
            }
        }
    }
}