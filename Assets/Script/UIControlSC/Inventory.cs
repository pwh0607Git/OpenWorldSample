using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal.Profiling.Memory.Experimental;
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
        itemQueue = new Queue<ItemData>();
        inventoryItemIconManager = GetComponent<InventoryItemIconManager>();
    }

    public GameObject slotPrefab;
    public Transform scrollContent;                 //inventorySlot의 부모객체

    private Queue<ItemData> itemQueue;              //먹은 아이템에 대한 순서가 보장되어야하기 때문...

    public List<InventorySlot> slots;
    public int maxSlotSize;

    private void OnEnable()
    {
        SyncUIData();
    }

    void CreateInventorySlot()
    {
        int columns = 4;

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
            rectTransform.anchoredPosition = new Vector2(startPosition.x + column * (componentSize.x), startPosition.y - row * (componentSize.y));
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

        if (gameObject.activeSelf)
        {
            itemQueue.Enqueue(itemData);
            SyncUIData();
        }
        else
        {
            //Queue에 넣기..
            itemQueue.Enqueue(itemData);
        }
    }

    //장비창이 활성화 되었을 때, 동기화하기.
    public void SyncUIData()
    {
        while (itemQueue.Count >= 0)
        {
            ItemData newitemData = itemQueue.Dequeue();

            if (newitemData != null)
            {
                //아이템이 존재하지 않으면 아이템 아이콘 생성.
                //먹은 아이템 구분하기
                if (newitemData != null)
                {
                    if (newitemData is Consumable consumable)
                    {
                        if (SearchItemByType<ConsumableType>(newitemData.itemType, consumable.subType))
                        {
                            Debug.Log($"{consumable.subType}은 인벤토리에 존재...");
                            consumable.GetThisItem();
                            //return;
                        }
                        else
                        {
                            GetNewItem(newitemData);
                        }
                    }
                    else if (newitemData is Equipment equipment)
                    {
                        GetNewItem(newitemData);
                    }
                }
                else
                {
                    Debug.Log("ItemData is Null...");
                }
            }
        }
    }

    //inventory에 존재하지 않은 아이템 GET
    public void GetNewItem(ItemData newItem)
    {
        InventorySlot emptySlot = GetEmptyInventorySlot();
        inventoryItemIconManager.CreateItemIcon(newItem, emptySlot);
    }

    public void UpdateInventorySlots()
    {
        SyncUIData();
    }
}