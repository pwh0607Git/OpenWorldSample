using UnityEngine;
using UnityEngine.EventSystems;

//ItemIconController�� ���� ���.
public class ItemContoller : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
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
        rectTransform.SetParent(transform.root);                        // �������� �ֻ����� �̵� (canvas)
        canvasGroup.blocksRaycasts = false;                             // �巡�� �� ����� �������� ����
        currentItemData = GetComponent<ItemDataSC>().GetItem;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ItemData itemData = GetComponent<ItemDataSC>().GetItem;
        bool isPresetting = false;

        if (itemData != null && itemData is Consumable consumable)
        {
            isPresetting = consumable.isPresetting;
        }

        originalParent.GetComponent<DragAndDropSlot>().CleanCurrentItem();

        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter != null)
        {
            //ActionBar�� ���Դµ� ���� �������� �Һ� �������� �ƴϸ� ���� �ڸ��� ����
            if(itemData is Consumable consumableItem && eventData.pointerEnter.tag != "ActionBarSlot")
            {
                ResetToOriginalSlot();
            }else if(itemData is Equipment equipmentItem && eventData.pointerEnter.tag != "EquipmentSlot")
            {
                ResetToOriginalSlot();
            }
            //��� ĭ�� ���Դµ� ���� �������� ��� �������� �ƴ϶��

            if (originalParent.tag == "ActionBarSlot")
            {
                if (eventData.pointerEnter.tag == "ActionBarSlot")                      //������ -> �� ������(�̵�)
                {
                    TransformItemIcon(eventData.pointerEnter.transform);
                }
                else if (eventData.pointerEnter.tag == "InventorySlot")                  //������ -> ����(���濡 �ش� �������� �����ϸ� �ش� ������ ����.
                {
                    Destroy(gameObject);
                }
                else if (eventData.pointerEnter.tag == "Item")                           //������ item1 -> ������ item2 (��ü)
                {
                    if (eventData.pointerEnter.transform.parent.tag == "ActionBarSlot")
                    {
                        SwapItemIcon(transform, eventData.pointerEnter.transform);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }

            }
            else if (originalParent.tag == "InventorySlot")
            {
                if (eventData.pointerEnter.tag == "ActionBarSlot")                      //���� -> ������ (����)
                {
                    if (isPresetting)
                    {
                        ResetToOriginalSlot();
                    }
                    else
                    {
                        isPresetting = true;
                        DuplicateItemIcon(eventData.pointerEnter.transform);
                    }
                }
                else if (eventData.pointerEnter.tag == "InventorySlot" || eventData.pointerEnter.tag == "EquipmentSlot")                 //���� -> �� ���� ����(�̵�)
                {
                    TransformItemIcon(eventData.pointerEnter.transform);
                    if (eventData.pointerEnter.tag == "EquipmentSlot")
                    {
                        itemData.Use();
                    }
                }
                else if (eventData.pointerEnter.tag == "Item")                          //���� �� item1 -> ���� �� item2 (��ü)
                {
                    if (eventData.pointerEnter.transform.parent.tag == "InventorySlot")
                    {
                        SwapItemIcon(transform, eventData.pointerEnter.transform);
                    }
                    else if (eventData.pointerEnter.transform.parent.tag == "ActionBarSlot")
                    {
                        Destroy(eventData.pointerEnter);
                        DuplicateItemIcon(transform);
                    }
                    else if (eventData.pointerEnter.transform.parent.tag == "EquipmentSlot")
                    {
                        //��� ��ȯ ��� �߰��ϱ�. => Swap �� ���� ������ [Ż��], ���� ������[����]
                        //���Կ��� �����ϰ� �ִ� ������
                        Equipment equipedItem = eventData.pointerEnter.GetComponent<EquipmentItemSC>().GetItem as Equipment;
                        equipedItem.Detach();

                        //�ٲ� ������
                        Equipment newItem = GetComponent<EquipmentItemSC>().GetItem as Equipment;
                        newItem.Use();

                        SwapItemIcon(transform, eventData.pointerEnter.transform);
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else if (originalParent.tag == "EquipmentSlot")
            {
                if (eventData.pointerEnter.transform.tag == "InventorySlot")
                {
                    State myState = PlayerController.player.myState;
                    if (itemData is Equipment equipment)
                    {
                        TransformItemIcon(eventData.pointerEnter.transform);
                        equipment.Detach();
                    }
                }
            }
        }
        else
        {
            if (originalParent.tag == "ActionBarSlot")          //������ -> ������ �ٸ� ����
            {
                Destroy(gameObject);
            }
            else if (originalParent.tag == "InventorySlot")
            {
                ResetToOriginalSlot();
            }
        }

        originalParent.GetComponent<DragAndDropSlot>().AssignCurrentItem(gameObject);
    }

    //�巡�� Begin�� �Ҵ� ����, End�� �Ҵ�.
    private void TransformItemIcon(Transform slot)
    {
        transform.SetParent(slot.transform); 
    }

    private void SwapItemIcon(Transform item1, Transform item2)
    {
        Transform newParent = item2.parent;
        item1.SetParent(newParent);
        item2.SetParent(originalParent);
        item2.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private void DuplicateItemIcon(Transform newTransform)
    {
        GameObject iconInstance = Instantiate(transform.gameObject);
        iconInstance.transform.SetParent(newTransform);
        iconInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        ResetToOriginalSlot();

        ItemData itemData = GetComponent<ItemDataSC>().GetItem;
 
        if (itemData != null && itemData is Consumable consumable)
        {
            consumable.isPresetting = true;
        }
    }

    private void ResetToOriginalSlot()
    {
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = Vector2.zero;
    }

    private void OnDestroy()
    {
        ItemData itemData = GetComponent<ItemDataSC>().GetItem;

        if (itemData != null && itemData is Consumable consumable)
        {
            consumable.isPresetting = false;
        }
    }
}