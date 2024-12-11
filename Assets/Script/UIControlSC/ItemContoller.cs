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
        rectTransform.SetParent(transform.root);                // 아이템을 최상위로 이동 (canvas)
        canvasGroup.blocksRaycasts = false;                     // 드래그 중 드롭이 가능한지 설정
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

        canvasGroup.blocksRaycasts = true;
        
        if (eventData.pointerEnter != null)
        {
            if(originalParent.tag == "ActionBarSlot")
            {
                if (eventData.pointerEnter.tag == "ActionBarSlot")                      //프리셋 -> 빈 프리셋(이동)
                {
                    TransformItemIcon(eventData.pointerEnter.transform);
                }
                else if(eventData.pointerEnter.tag == "InventorySlot")                  //프리셋 -> 가방(가방에 해당 아이템이 존재하면 해당 아이콘 삭제.
                {
                    Destroy(gameObject);
                }
                else if(eventData.pointerEnter.tag == "Item")                           //프리셋 item1 -> 프리셋 item2 (교체)
                {
                    if(eventData.pointerEnter.transform.parent.tag == "ActionBarSlot")
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
            else if(originalParent.tag == "InventorySlot")
            {
                if (eventData.pointerEnter.tag == "ActionBarSlot")                      //가방 -> 프리셋 (복제)
                {
                    if (isPresetting) 
                    { 
                        transform.SetParent(originalParent);
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
                    if(eventData.pointerEnter.transform.parent.tag == "InventorySlot")
                    {
                        SwapItemIcon(transform, eventData.pointerEnter.transform);
                    }
                    else if(eventData.pointerEnter.transform.parent.tag == "ActionBarSlot")
                    {
                        Destroy(eventData.pointerEnter);
                        DuplicateItemIcon(transform);
                    }
                    //장비 교환 기능 추가하기. => Swap 후 기존 데이터 [탈착], 이후 데이터[착용]
                }
                else
                {
                    Destroy(gameObject);
                }
            }else if(originalParent.tag == "EquipmentSlot")
            {
                Debug.Log("탈착 프로세스1");
                //조건
                /*
                 장비 슬롯 -> 가방[이동]
                 장비 슬롯 -> 가방의 장비[교환]
                 장비 슬롯 -> 완전 외부 [원래 장비로 돌려두기]
                 장비 슬롯 -> 가방속 아이템(장비 아이템이 아닌 경우.. 소비 혹은 기타 아이템), 빈공간 아무곳에 배치
                */

                if (eventData.pointerEnter.transform.tag == "InventorySlot")
                {
                    State myState = PlayerController.player.myState;
                    if (itemData is Equipment equipment)
                    {
                        Debug.Log("탈착 프로세스2");
                        TransformItemIcon(eventData.pointerEnter.transform);

                    }
                    myState.DetachItem((Equipment)itemData);
                    Debug.Log("탈착 프로세스3");
                }
            }
        }
        else
        {
            if(originalParent.tag == "ActionBarSlot")          //프리셋 -> 완전히 다른 공간
            {
                Destroy(gameObject);
            }else if (originalParent.tag == "InventorySlot")
            {
                transform.SetParent(originalParent);
            }
        }

        rectTransform.anchoredPosition = Vector2.zero;
    }

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
        transform.transform.SetParent(originalParent);

        ItemData itemData = GetComponent<ItemDataSC>().GetItem;
 
        if (itemData != null && itemData is Consumable consumable)
        {
            consumable.isPresetting = true;
        }
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