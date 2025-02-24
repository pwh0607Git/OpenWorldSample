using UnityEngine;
using UnityEngine.EventSystems;

// Chase : 아이콘 이동 로직만 수행
public class ItemIconController : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private ItemDataHandler itemDataHandler;
    private Transform originalParent;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        itemDataHandler = GetComponent<ItemDataHandler>();
    }

    void Start()
    {
        rectTransform.anchoredPosition = Vector2.zero;
    }


    private float clickTimer = 0.0f;
    private float doubleClickTime = 0.3f;
    public void OnPointerClick(PointerEventData eventData)
    {
        DragAndDropSlot slot = transform.GetComponentInParent<DragAndDropSlot>();

        if(slot == null) return;
        //액션바 슬롯에서 우클릭시, 액션바에 할당 삭제.
        if (eventData.button == PointerEventData.InputButton.Right && slot is ActionBarSlot)
        {
            Destroy(transform.gameObject);
        }

        //좌클릭 더블클릭.
        if(eventData.button == PointerEventData.InputButton.Left){
            if(Time.time - clickTimer < doubleClickTime){
                Debug.Log("아이콘 더블 클릭!"); 
                if(slot is InventorySlot && itemDataHandler.GetItem is Consumable consumable){
                    //소비아이템 소모
                    consumable.Use();
                }
            }
            clickTimer = Time.time;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        rectTransform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragAndDropSlot targetSlot = eventData.pointerEnter?.GetComponentInParent<DragAndDropSlot>();
        originalParent.GetComponent<DragAndDropSlot>().ClearCurrentItem();

        if(targetSlot == null || !targetSlot.CheckVaildItem<ItemType>(gameObject)){
            ResetToOriginalSlot();
            return;
        }

        DragAndDropSlot originalSlot = originalParent.GetComponent<DragAndDropSlot>();
        originalSlot.ClearCurrentItem();

        if (originalSlot is InventorySlot && targetSlot is ActionBarSlot)
        {
            // 타겟 슬롯에 복사

            // if (!HandleInventorySlot(dropTarget, itemData))
            // {
            //     ResetToOriginalSlot();
            //     return;
            // }
        }
        else if (originalSlot is ActionBarSlot && targetSlot is ActionBarSlot)
        {
            if(targetSlot.currentItem != null){
                // 타겟 Slot에 아이템이 할당되어있으면 Swap
                targetSlot.SwapItem(this.gameObject);
            }else{
                // 없으면 그냥할당. 
                targetSlot.AssignCurrentItem(this.gameObject);
            }
            // if (!HandleActionBarSlot(dropTarget)) {
            //     ResetToOriginalSlot();
            //     return;
            // } 
        }
        else{

        }
        // else if (originalParent.GetComponent<DragAndDropSlot>() is EquipmentSlot)
        // {
        //     // if(!HandleEquipmentSlot(dropTarget, itemData))
        //     // {
        //     //     ResetToOriginalSlot();
        //     //     return;
        //     // }
        // }
        
        // dropTarget.GetComponent<DragAndDropSlot>().AssignCurrentItem(gameObject);
    }

    private bool HandleInventorySlot(Transform dropTarget, ItemData itemData)
    {
        DragAndDropSlot dropSlot = dropTarget.GetComponent<DragAndDropSlot>();

        if (dropSlot == null)
        {
            dropSlot = dropTarget.GetComponentInParent<DragAndDropSlot>();
            if (dropSlot == null) return false;
        }

        bool hasItem = dropSlot.currentItem != null;

        if (dropSlot is ActionBarSlot actionBarSlot)
        {
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
                TransformItemIcon(dropTarget);
            }
            else
            {
                Transform changeItem = dropTarget.GetComponentInChildren<ItemIconController>().transform;
                SwapItemIcon(transform, changeItem);
            }
        }
        else if (dropSlot is EquipmentSlot equipmentSlot)
        {
            if (!equipmentSlot.CheckEquipmentItem(gameObject)) return false;

            if (equipmentSlot.CheckEquipmentItem(gameObject))
            {
                if (!hasItem)
                {
                    TransformItemIcon(dropTarget);
                    itemData.Use();
                }
                else
                {
                    Equipment equipedItem = dropTarget.GetComponentInChildren<ItemIconController>().GetComponent<EquipmentItemHandler>().GetItem as Equipment;
                    equipedItem.Detach();

                    Equipment newItem = GetComponent<EquipmentItemHandler>().GetItem as Equipment;
                    newItem.Use();

                    Transform changeItem = dropTarget.GetComponentInChildren<ItemIconController>().transform;
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

    private bool HandleActionBarSlot(Transform dropTarget)
    {
        DragAndDropSlot dropSlot = dropTarget.GetComponent<DragAndDropSlot>();
        
        if (dropSlot == null)
        {
            dropSlot = dropTarget.GetComponentInParent<DragAndDropSlot>();

            if (dropSlot == null) return false;
        }

        bool hasItem = dropSlot.currentItem != null;

        if (dropSlot is ActionBarSlot)
        {
            if(!hasItem)
            {
                TransformItemIcon(dropTarget);
            }
            else
            {
                Transform changeItem = dropTarget.GetComponentInChildren<ItemIconController>().transform;            //������ ������ ������Ʈ�� �������ִ� ������Ʈ�� Transform ��������
                SwapItemIcon(transform, changeItem);
            }
        }else if(dropSlot is InventorySlot)
        {
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
           dropSlot = dropTarget.GetComponentInParent<DragAndDropSlot>();

            if (dropSlot == null) return false;
        }

        if (dropSlot is InventorySlot)
        {
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

    private void TransformItemIcon(Transform slot)
    {
        transform.SetParent(slot.transform);
    }

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
        ResetToOriginalSlot();

        Consumable itemData = GetComponent<ItemDataHandler>().GetItem as Consumable;
        itemData.isPresetting = true;
    }

    private void ResetToOriginalSlot()
    {
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = Vector2.zero;
        canvasGroup.blocksRaycasts = true;
        // originalParent.transform.GetComponent<DragAndDropSlot>().AssignCurrentItem(gameObject);
    }

    private void OnDestroy()
    {
        Consumable itemData = GetComponent<ItemDataHandler>().GetItem as Consumable;
        itemData.isPresetting = false;
    }

    public DragAndDropSlot GetSlot(Transform dropTarget)
    {
        DragAndDropSlot slot = dropTarget.GetComponent<DragAndDropSlot>();

        if (slot == null)
        {
            slot = dropTarget.GetComponentInParent<DragAndDropSlot>();

            if (slot == null) return null;
        }
        return slot;
    }
}