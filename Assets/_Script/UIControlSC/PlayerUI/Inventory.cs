using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private void Awake()
    {
        CreateInventorySlot();
        itemQueue = new Queue<ItemData>();
        inventoryItemIconManager = GetComponent<InventoryItemIconManager>();
    }

    public GameObject slotPrefab;
    public Transform scrollContent;                 //inventorySlot�� �θ�ü

    private Queue<ItemData> itemQueue;             

    public List<InventorySlot> slots;
    public int maxSlotSize;
        
    private void OnEnable()
    {
        SyncUIData();
    }

    private void CreateInventorySlot()
    {
        int columns = 4;

        Vector2 startPosition = new Vector2(-120f, -50f);
        Vector2 componentSize = new Vector2(80f, 80f);

        for (int i = 0; i < maxSlotSize; i++)
        {
            GameObject slotInstance = Instantiate(slotPrefab);
            slotInstance.transform.SetParent(scrollContent);

            slots.Add(slotInstance.GetComponent<InventorySlot>());

            int row = i / columns;
            int column = i % columns;

            RectTransform rectTransform = slotInstance.GetComponent<RectTransform>();
            rectTransform.sizeDelta = componentSize;
            rectTransform.localScale = Vector2.one;
            rectTransform.anchoredPosition = new Vector2(startPosition.x + column * (componentSize.x), startPosition.y - row * (componentSize.y));
        }
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

            if (slotItemData.itemType == itemType)
            {
                if (subType == null)
                {
                    Debug.Log("Search Code : 001");
                    return true;           //��Ÿ ������
                }

                if (itemType == ItemType.Consumable && slotItemData is Consumable consumable)
                {
                    if (EqualityComparer<T>.Default.Equals((T)(object)consumable.subType, subType.Value))
                    {
                        Debug.Log("Search Code : 002");
                        return true;
                    }
                }
                else if (itemType == ItemType.Equipment && slotItemData is Equipment equipment)
                {
                    if (EqualityComparer<T>.Default.Equals((T)(object)equipment.subType, subType.Value))
                    {
                        Debug.Log("Search Code : 003");
                        return true;
                    }
                }
            }
        }
        Debug.Log("Search Code : 004");
        return false;
    }
    
    public void GetItem(GameObject item)
    {
        ItemDataSC itemDataSC = item.GetComponent<ItemDataSC>();            //DroppedItemSC�� �����Ѵ�...
        ItemData itemData = itemDataSC.GetItem;
        itemQueue.Enqueue(itemData);

        if (gameObject.activeSelf) {
            SyncUIData();       //�κ��丮 â�� Ȱ��ȭ�Ǿ� ������ �ٷ� ���.
        }
    }

    public void SyncUIData()
    {
        while (itemQueue.Count > 0)
        {
            ItemData newItemData = itemQueue.Dequeue();

            if (newItemData != null)
            {
                if (newItemData is Consumable consumable)
                {
                    if (SearchItemByType<ConsumableType>(newItemData.itemType, consumable.subType))
                    {
                        consumable.GetThisItem();
                    }
                    else
                    {
                        GetNewItem(newItemData);
                    }
                }
                else if (newItemData is Equipment equipment)
                {
                    GetNewItem(newItemData);
                }
            }
            else
            {
                Debug.Log("ItemData is Null...");
            }
        }
    }

    public void GetNewItem(ItemData newItem)
    {
        InventorySlot emptySlot = GetEmptyInventorySlot();
        inventoryItemIconManager.CreateItemIcon(newItem, emptySlot);
    }

    //������ ����.
    public Transform originalParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        GetComponent<RectTransform>().SetParent(transform.root);                // �������� �ֻ����� �̵� (canvas)
        GetComponent<CanvasGroup>().blocksRaycasts = false;                     // �巡�� �� ����� �������� ����
    }

    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true; 
        GetComponent<RectTransform>().SetParent(originalParent);
    }
}