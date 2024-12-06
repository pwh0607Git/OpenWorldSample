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
        if (transform.parent.tag == "KeyBoardPreSet_DragAndDropArea" && eventData.button == PointerEventData.InputButton.Right)
        {
            Destroy(transform.gameObject);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        rectTransform.SetParent(transform.root);        // 아이템을 최상위로 이동 (canvas)
        canvasGroup.blocksRaycasts = false;             // 드래그 중 드롭이 가능한지 설정
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
            if(originalParent.tag == "KeyBoardPreSet_DragAndDropArea")
            {
                if (eventData.pointerEnter.tag == "KeyBoardPreSet_DragAndDropArea")     //프리셋 -> 빈 프리셋(이동)
                {
                    Debug.Log("Type01");
                    TransformItemIcon(eventData.pointerEnter.transform);
                }
                else if(eventData.pointerEnter.tag == "Bag_DragAndDropArea")                    //프리셋 -> 가방(가방에 해당 아이템이 존재하면 해당 아이콘 삭제.
                {
                    Debug.Log("Type02");
                    Destroy(gameObject);
                }
                else if(eventData.pointerEnter.tag == "Item")                                   //프리셋 item1 -> 프리셋 item2 (교체)
                {
                    if(eventData.pointerEnter.transform.parent.tag == "KeyBoardPreSet_DragAndDropArea")
                    {
                        Debug.Log("Type03");
                        SwapItemIcon(transform, eventData.pointerEnter.transform);
                    }
                    else
                    {   
                        Debug.Log("Type04");
                        Destroy(gameObject);
                    }
                }
                else
                {
                    //삭제
                    Debug.Log("Type05");
                    Destroy(gameObject);
                }

            }
            else if(originalParent.tag == "Bag_DragAndDropArea")
            {
                if (eventData.pointerEnter.tag == "KeyBoardPreSet_DragAndDropArea")     //가방 -> 프리셋 (복제)
                {
                    Debug.Log("Type06");
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
                else if (eventData.pointerEnter.tag == "Bag_DragAndDropArea")                    //가방 -> 빈 가방 공간(이동)
                {
                    Debug.Log("Type07");
                    TransformItemIcon(eventData.pointerEnter.transform);
                }
                else if (eventData.pointerEnter.tag == "Item")                                   //가방 속 item1 -> 가방 속 item2 (교체)
                {
                    if(eventData.pointerEnter.transform.parent.tag == "Bag_DragAndDropArea")
                    {
                        //가방 안의 Item라면..
                        Debug.Log("Type08");
                        SwapItemIcon(transform, eventData.pointerEnter.transform);
                    }
                    else if(eventData.pointerEnter.transform.parent.tag == "KeyBoardPreSet_DragAndDropArea")
                    {
                        //프리셋 안의 아이템이라면 프리셋 안의 오브젝트를 삭제하고 가져온 오브젝트를 복제한다.
                        Debug.Log("Type09");
                        Destroy(eventData.pointerEnter);
                        DuplicateItemIcon(transform);
                    }
                }
                else
                {
                    Debug.Log("Type10");
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if(originalParent.tag == "KeyBoardPreSet_DragAndDropArea")          //프리셋 -> 완전히 다른 공간
            {
                Destroy(gameObject);
            }else if (originalParent.tag == "Bag_DragAndDropArea")
            {
                transform.SetParent(originalParent);
            }
        }

        rectTransform.anchoredPosition = Vector2.zero;
    }

    private void TransformItemIcon(Transform item)
    {
        transform.SetParent(item.transform);
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