using UnityEngine;
using UnityEngine.EventSystems;

//IPointerClickHandler,
public class ItemIconController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private ItemDataHandler itemDataHandler;
    private CanvasGroup canvasGroup;
    public DragAndDropSlot originalSlot;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        itemDataHandler = GetComponent<ItemDataHandler>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        originalSlot = transform.GetComponentInParent<DragAndDropSlot>();
        gameObject.transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void ResetToOriginalSlot(){
        gameObject.transform.SetParent(originalSlot.transform);
        rectTransform.anchoredPosition = Vector2.zero;
    }
}