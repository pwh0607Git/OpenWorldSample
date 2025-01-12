using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//ItemIconController로 변경 요망.
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
        bool isPresetting = false;

        if (itemData != null && itemData is Consumable consumable)
        {
            isPresetting = consumable.isPresetting;
        }

        originalParent.GetComponent<DragAndDropSlot>().CleanCurrentItem();

        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter != null)
        {
            //ActionBar에 들어왔는데 현재 아이템이 소비 아이템이 아니면 원래 자리로 복귀
            if(itemData is Consumable consumableItem && eventData.pointerEnter.tag != "ActionBarSlot")
            {
                ResetToOriginalSlot();
            }else if(itemData is Equipment equipmentItem && eventData.pointerEnter.tag != "EquipmentSlot")
            {
                ResetToOriginalSlot();
            }
            //장비 칸에 들어왔는데 현재 아이템이 장비 아이템이 아니라면

            if (originalParent.tag == "ActionBarSlot")
            {
                if (eventData.pointerEnter.tag == "ActionBarSlot")                      //프리셋 -> 빈 프리셋(이동)
                {
                    TransformItemIcon(eventData.pointerEnter.transform);
                }
                else if (eventData.pointerEnter.tag == "InventorySlot")                  //프리셋 -> 가방(가방에 해당 아이템이 존재하면 해당 아이콘 삭제.
                {
                    Destroy(gameObject);
                }
                else if (eventData.pointerEnter.tag == "Item")                           //프리셋 item1 -> 프리셋 item2 (교체)
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
                if (eventData.pointerEnter.tag == "ActionBarSlot")                      //가방 -> 프리셋 (복제)
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
                else if (eventData.pointerEnter.tag == "InventorySlot" || eventData.pointerEnter.tag == "EquipmentSlot")                 //가방 -> 빈 가방 공간(이동)
                {
                    TransformItemIcon(eventData.pointerEnter.transform);
                    if (eventData.pointerEnter.tag == "EquipmentSlot")
                    {
                        itemData.Use();
                    }
                }
                else if (eventData.pointerEnter.tag == "Item")                          //가방 속 item1 -> 가방 속 item2 (교체)
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
                        //장비 교환 기능 추가하기. => Swap 후 기존 데이터 [탈착], 이후 데이터[착용]
                        //슬롯에서 장착하고 있는 아이템
                        Equipment equipedItem = eventData.pointerEnter.GetComponent<EquipmentItemSC>().GetItem as Equipment;
                        equipedItem.Detach();

                        //바꿀 아이템
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
            if (originalParent.tag == "ActionBarSlot")          //프리셋 -> 완전히 다른 공간
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

    //드래그 Begin시 할당 제거, End시 할당.
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