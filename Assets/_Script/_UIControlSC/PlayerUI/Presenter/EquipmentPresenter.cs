using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentPresenter :MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public EquipmentModel model;
    public EquipmentView view;

    public EquipmentPresenter(EquipmentModel model, EquipmentView view){
        this.model = model;
        this.view = view;
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