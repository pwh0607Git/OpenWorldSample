using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardSlot : MonoBehaviour, IDropHandler
{
    public KeyCode assignedKey;
    public GameObject currentItem { get; set; }             //현재 프리셋에 존재하는 아이템

    void Update()
    {
        if (transform.childCount == 0)
        {
            currentItem = null;
        }

        if (Input.GetKeyDown(assignedKey))
        {
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
        Debug.Log($"{item} 할당..");
        currentItem = item;
        Keyboard.myKeyboard.UpdateKeyboardPreset();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;                                 //드래그 상태인 item 참조.
        AssignItem(droppedItem);
    }
}