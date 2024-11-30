using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemContoller : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
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
        //preset에서의 우클릭시 삭제하기.
        if (transform.parent.tag == "KeyBoardPreSet_DragAndDropArea" && eventData.button == PointerEventData.InputButton.Right)
        {
            Destroy(transform.gameObject);
        }
    }

    // 드래그 시작 시 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        rectTransform.SetParent(transform.root);  // 아이템을 최상위로 이동 (canvas)
        canvasGroup.blocksRaycasts = false;       // 드래그 중 드롭이 가능한지 설정
    }

    // 드래그 중 호출
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        Debug.Log($"{eventData.pointerEnter.tag}");
        if (eventData.pointerEnter != null)
        {
            if (eventData.pointerEnter.tag == "KeyBoardPreSet_DragAndDropArea")
            {
                DuplicateItemIcon(eventData.pointerEnter.transform);
            }else if (eventData.pointerEnter.tag == "Bag_DragAndDropArea")
            {
                transform.SetParent(eventData.pointerEnter.transform);
            }
            else if (eventData.pointerEnter.tag == "Item")
            {
                //swap
                Transform item1 = transform;
                Transform item2 = eventData.pointerEnter.transform;
                Debug.Log($"{eventData.pointerEnter.name} Swap!");
                SwapItemInBag(item1, item2);
            }
            else
            {
                transform.SetParent(originalParent);
            }
        }
        else
        {
            transform.SetParent(originalParent);
        }
        rectTransform.anchoredPosition = Vector2.zero;
    }
    
    void SwapItemInBag(Transform item1, Transform item2)
    {
        Transform newParent = item2.parent;
        if (item1.parent.tag == "Bag_DragAndDropArea" && item2.parent.tag == "KeyBoardPreSet_DragAndDropArea")
        {
            //삭제.
            Destroy(item2.gameObject);      //기존의 항목을 삭제.
            DuplicateItemIcon(newParent);
        }
        else if(item1.parent.tag == "KeyBoardPreSet_DragAndDropArea" && item2.parent.tag == "KeyBoardPreSet_DragAndDropArea")
        {
            item1.SetParent(newParent);
            item2.SetParent(originalParent);
            item2.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    void DuplicateItemIcon(Transform newTransform)
    {
        GameObject iconInstance = Instantiate(transform.gameObject);
        iconInstance.transform.SetParent(newTransform);
        transform.transform.SetParent(originalParent);
    }
}
