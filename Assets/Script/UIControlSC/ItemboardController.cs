using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemboardController : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.gameObject.name);
        GameObject droppedItem = eventData.pointerDrag;         //드래그 상태인 item 참조.
        if (droppedItem != null && droppedItem.GetComponent<ItemContoller>() != null)
        {
            droppedItem.transform.SetParent(transform);
            droppedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            
            //들어온 아이템에 따라 키보드 이벤트 추가하기.
        }
    }
}

/*
 public class KeyPreset : MonoBehaviour
{
    public KeyCode assignedKey;
    private GameObject currentItem;

    void Update()
    {
        if (currentItem != null && Input.GetKeyDown(assignedKey))
        {
            UseItem();
        }
    }

    public void AssignItem(GameObject item)
    {
        currentItem = item;
    }

    private void UseItem()
    {
        Debug.Log("사용된 아이템: " + currentItem.name);
        Destroy(currentItem);  // 예시로 아이템 삭제
    }
}
 */