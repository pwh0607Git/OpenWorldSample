using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentWindow : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject equipmentSlotContent;

    public List<EquipmentSlot> slots;

    private void Start()
    {
        slots = new List<EquipmentSlot>();
    }

    //슬롯창 초기화
    private void AddEquipmentSlotRef()
    {
        foreach(Transform child in equipmentSlotContent.transform)
        {
            EquipmentSlot slot = child.GetComponent<EquipmentSlot>();
            if(slot != null)
            {
                slots.Add(slot);
            }
        }
    }

    public Transform originalParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        GetComponent<RectTransform>().SetParent(transform.root);                // 아이템을 최상위로 이동 (canvas)
        GetComponent<CanvasGroup>().blocksRaycasts = false;                     // 드래그 중 드롭이 가능한지 설정
    }

    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        GetComponent<RectTransform>().SetParent(originalParent);
    }
}
