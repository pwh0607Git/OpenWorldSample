using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemIconController : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Transform originalParent;
    private ItemData currentItemData;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        rectTransform.anchoredPosition = Vector2.zero;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.parent.tag == "ActionBarSlot" && eventData.button == PointerEventData.InputButton.Right)
        {
            Destroy(transform.gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        rectTransform.SetParent(transform.root);                        // 아이템을 최상위로 이동 (canvas)
        canvasGroup.blocksRaycasts = false;                             // 드래그 중 드롭이 가능한지 설정
        currentItemData = GetComponent<ItemDataSC>().GetItem;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ItemData itemData = GetComponent<ItemDataSC>().GetItem;
        Transform dropTarget = eventData.pointerEnter?.transform;

        originalParent.GetComponent<DragAndDropSlot>().CleanCurrentItem();

        canvasGroup.blocksRaycasts = true;

        // 슬롯별 처리
        if (originalParent.GetComponent<DragAndDropSlot>() is InventorySlot)
        {
            if (!HandleInventorySlot(dropTarget, itemData))
            {
                ResetToOriginalSlot();
                return;
            }
        }
        else if (originalParent.GetComponent<DragAndDropSlot>() is ActionBarSlot)
        {
            if (!HandleActionBarSlot(dropTarget)) {
                ResetToOriginalSlot();
                return;
            } 
        }
        else if (originalParent.GetComponent<DragAndDropSlot>() is EquipmentSlot)
        {
            if(!HandleEquipmentSlot(dropTarget, itemData))
            {
                ResetToOriginalSlot();
                return;
            }
        }

        // 슬롯 동기화
        dropTarget.GetComponent<DragAndDropSlot>().AssignCurrentItem(gameObject);
    }

    private bool HandleInventorySlot(Transform dropTarget, ItemData itemData)
    {
        DragAndDropSlot dropSlot = dropTarget.GetComponent<DragAndDropSlot>();

        if (dropSlot == null)
        {
            //dropTarget 자체에 슬롯이 없으면 부모 계층에서 가져오기
            dropSlot = dropTarget.GetComponentInParent<DragAndDropSlot>();

            if (dropSlot == null) return false;
        }

        bool hasItem = dropSlot.GetCurrentItem() != null;

        //Inventory 기준
        if (dropSlot is ActionBarSlot actionBarSlot)
        {
            //Inventory -> ActionBar
            Debug.Log("Inventory -> ActionBar");
            if (itemData is Consumable consumable && consumable.isPresetting)
            {
                return false;
            }
            else
            {
                DuplicateItemIcon(dropTarget);
            }
        }
        else if (dropSlot is InventorySlot inventorySlot)
        {
            if (!hasItem)
            {
                //Inventory -> Inventory[빈]
                Debug.Log("Inventory -> Inventory[빈]");
                TransformItemIcon(dropTarget);
            }
            else
            {
                //Inventory -> Inventory[아이템이 할당 된 Slot]
                Debug.Log("Inventory -> Inventory[아이템이 할당 된 Slot]");
                Transform changeItem = dropTarget.GetComponentInChildren<ItemIconController>().transform;            //아이템 아이콘 컴포넌트를 가지고있는 오브젝트의 Transform 가져오기

                SwapItemIcon(transform, changeItem);
            }
        }
        else if (dropSlot is EquipmentSlot equipmentSlot)
        {
            if (!equipmentSlot.CheckEquipmentItem(gameObject))
            {
                Debug.Log($"{gameObject.name}은 장비 아이템이 아닙니다.");
                return false;
            }

            if (equipmentSlot.CheckEquipmentItem(gameObject))
            {
                //빈슬롯 인경우...
                if (!hasItem)
                {
                    Debug.Log("Inventory -> Equipment[빈 슬롯]");
                    TransformItemIcon(dropTarget);
                    itemData.Use();
                }
                else
                {
                    Debug.Log("Inventory -> Equipment[할당된 슬롯]");
                    //아이템이 할당되어있는 경우..
                    Equipment equipedItem = dropTarget.GetComponentInChildren<ItemIconController>().GetComponent<EquipmentItemSC>().GetItem as Equipment;
                    equipedItem.Detach();

                    //바꿀 아이템
                    Equipment newItem = GetComponent<EquipmentItemSC>().GetItem as Equipment;
                    newItem.Use();

                    Transform changeItem = dropTarget.GetComponentInChildren<ItemIconController>().transform;            //아이템 아이콘 컴포넌트를 가지고있는 오브젝트의 Transform 가져오기
                    SwapItemIcon(transform, changeItem);
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    //각각의 슬롯 처리
    private bool HandleActionBarSlot(Transform dropTarget)
    {
        DragAndDropSlot dropSlot = dropTarget.GetComponent<DragAndDropSlot>();
        
        if (dropSlot == null)
        {
            //dropTarget 자체에 슬롯이 없으면 부모 계층에서 가져오기
            dropSlot = dropTarget.GetComponentInParent<DragAndDropSlot>();

            if (dropSlot == null) return false;
        }

        bool hasItem = dropSlot.GetCurrentItem() != null;
        //actionBar 기준
        if (dropSlot is ActionBarSlot)
        {
            if(!hasItem)
            {
                //ActionBar -> ActionBar[빈] : 이동
                Debug.Log("ActionBar -> ActionBar[빈]");
                TransformItemIcon(dropTarget);
            }
            else
            {
                //ActionBar -> ActionBar[아이템이 할당된 슬롯]
                Debug.Log("ActionBar -> ActionBar[아이템이 할당된 슬롯]");
                Transform changeItem = dropTarget.GetComponentInChildren<ItemIconController>().transform;            //아이템 아이콘 컴포넌트를 가지고있는 오브젝트의 Transform 가져오기
                SwapItemIcon(transform, changeItem);
            }
        }else if(dropSlot is InventorySlot)
        {
            //ActionBar -> Inventory
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        return true;
    }

    private bool HandleEquipmentSlot(Transform dropTarget, ItemData itemData)
    {
        DragAndDropSlot dropSlot = dropTarget.GetComponent<DragAndDropSlot>();

        if (dropSlot == null)
        {
            //dropTarget 자체에 슬롯이 없으면 부모 계층에서 가져오기
            dropSlot = dropTarget.GetComponentInParent<DragAndDropSlot>();

            if (dropSlot == null) return false;
        }

        //조건
        //Equipment -> Inventory[빈] : 이동
        if (dropSlot is InventorySlot)
        {
            // 장비 -> 인벤토리
            if (itemData is Equipment equipment)
            {
                TransformItemIcon(dropTarget);
                equipment.Detach();
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    //드래그 Begin시 할당 제거, End시 할당.
    private void TransformItemIcon(Transform slot)
    {
        transform.SetParent(slot.transform);
    }

    //item1은 현재 드래그한 아이템
    //item2은 슬롯에 들어있는 아이템
    private void SwapItemIcon(Transform item1, Transform item2)
    {
        item1.SetParent(item2.parent);

        DragAndDropSlot slot = GetSlot(originalParent);
        slot.AssignCurrentItem(item2.gameObject);
        item2.SetParent(originalParent);

        item2.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private void DuplicateItemIcon(Transform newTransform)
    {
        GameObject iconInstance = Instantiate(transform.gameObject);
        iconInstance.transform.SetParent(newTransform);
        iconInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        newTransform.GetComponent<DragAndDropSlot>().AssignCurrentItem(gameObject);

        //기존 아이템은 위치 리셋
        ResetToOriginalSlot();

        Consumable itemData = GetComponent<ItemDataSC>().GetItem as Consumable;
        itemData.isPresetting = true;
    }

    private void ResetToOriginalSlot()
    {
        Debug.Log($"아이템 위치 리셋");
        transform.SetParent(originalParent);
        originalParent.transform.GetComponent<DragAndDropSlot>().AssignCurrentItem(gameObject);
        rectTransform.anchoredPosition = Vector2.zero;
    }

    private void OnDestroy()
    {
        Consumable itemData = GetComponent<ItemDataSC>().GetItem as Consumable;
        itemData.isPresetting = false;
    }

    public DragAndDropSlot GetSlot(Transform dropTarget)
    {
        //아이템이 할당되어있는 슬롯을 가져오는 메서드.
        DragAndDropSlot slot = dropTarget.GetComponent<DragAndDropSlot>();

        if (slot == null)
        {
            //dropTarget 자체에 슬롯이 없으면 부모 계층에서 가져오기
            slot = dropTarget.GetComponentInParent<DragAndDropSlot>();

            if (slot == null) return null;
        }

        return slot;
    }
}