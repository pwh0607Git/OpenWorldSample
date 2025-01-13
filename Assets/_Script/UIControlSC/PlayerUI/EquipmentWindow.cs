using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentWindow : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject equipmentSlotContent;

    private List<EquipmentSlot> slots;

    private void Start()
    {
        slots = new List<EquipmentSlot>();
    }

    //����â �ʱ�ȭ
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
        GetComponent<RectTransform>().SetParent(transform.root);                // �������� �ֻ����� �̵� (canvas)
        GetComponent<CanvasGroup>().blocksRaycasts = false;                     // �巡�� �� ����� �������� ����
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
