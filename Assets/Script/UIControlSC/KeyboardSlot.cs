using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardSlot : MonoBehaviour, IDropHandler
{
    public KeyCode assignedKey;
    private GameObject currentItem;             //현재 프리셋에 존재하는 아이템

    void Update()
    {
        if (Input.GetKeyDown(assignedKey))
        {
            if (transform.childCount == 0)
            {
                currentItem = null;
            }

            if (currentItem != null)
            {
                UseItem();
            }
            else
            {
                Debug.Log("Null Item...");
            }
        }
    }

    void UseItem()
    {
        currentItem.GetComponent<ConsumableItemSC>().GetItem.Use();
    }

    private void AssignItem(GameObject item)
    {
        currentItem = item;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.gameObject.name);
        GameObject droppedItem = eventData.pointerDrag;                                 //드래그 상태인 item 참조.
        if (droppedItem != null && droppedItem.GetComponent<ItemContoller>() != null)
        {
            droppedItem.transform.SetParent(transform);
            droppedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            //들어온 아이템에 따라 키보드 이벤트 추가하기.
            AssignItem(droppedItem);
        }
    }
}