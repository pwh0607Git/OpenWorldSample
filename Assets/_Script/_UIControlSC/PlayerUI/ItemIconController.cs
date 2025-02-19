using UnityEngine;
using UnityEngine.EventSystems;

public class ItemIconController : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Transform originalParent;

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
        Debug.Log($"{gameObject.name} Drag Start");
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
        ItemData itemData = GetComponent<ItemDataSC>().GetItem;
        Transform dropTarget = eventData.pointerEnter?.transform;

        originalParent.GetComponent<DragAndDropSlot>().ClearCurrentItem();

        canvasGroup.blocksRaycasts = true;

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
        
        dropTarget.GetComponent<DragAndDropSlot>().AssignCurrentItem(gameObject);
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
                    Equipment equipedItem = dropTarget.GetComponentInChildren<ItemIconController>().GetComponent<EquipmentItemSC>().GetItem as Equipment;
                    equipedItem.Detach();

                    Equipment newItem = GetComponent<EquipmentItemSC>().GetItem as Equipment;
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

        Consumable itemData = GetComponent<ItemDataSC>().GetItem as Consumable;
        itemData.isPresetting = true;
    }

    private void ResetToOriginalSlot()
    {
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
        DragAndDropSlot slot = dropTarget.GetComponent<DragAndDropSlot>();

        if (slot == null)
        {
            slot = dropTarget.GetComponentInParent<DragAndDropSlot>();

            if (slot == null) return null;
        }
        return slot;
    }
}