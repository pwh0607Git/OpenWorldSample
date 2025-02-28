using UnityEngine;
using UnityEngine.EventSystems;

// Chase : 아이콘 이동 로직만 수행
public class ItemIconController : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private ItemDataHandler itemDataHandler;
    public DragAndDropSlot originalSlot;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        itemDataHandler = GetComponent<ItemDataHandler>();
    }

    void Start()
    {
        rectTransform.anchoredPosition = Vector2.zero;
        canvasGroup.blocksRaycasts = true;
    }

    private float clickTimer = 0.0f;
    private float doubleClickTime = 0.3f;
    public void OnPointerClick(PointerEventData eventData)
    {
        DragAndDropSlot slot = transform.GetComponentInParent<DragAndDropSlot>();
        if(slot == null) return;
        
        if (eventData.button == PointerEventData.InputButton.Right && slot is ActionBarSlot)
        {
            slot.ClearCurrentItem();
            Destroy(transform.gameObject);
        }

        //좌클릭 더블클릭.
        if(eventData.button == PointerEventData.InputButton.Left){
            if(Time.time - clickTimer < doubleClickTime){
                Debug.Log("아이콘 더블 클릭!"); 
                if(slot is InventorySlot && itemDataHandler.GetItem is Consumable consumable){
                    //소비아이템 소모
                    consumable.Use();
                }
            }
            clickTimer = Time.time;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalSlot = transform.GetComponentInParent<DragAndDropSlot>();
        originalSlot.ClearCurrentItem();
        rectTransform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragAndDropSlot targetSlot = eventData.pointerEnter?.GetComponentInParent<DragAndDropSlot>();
         if (targetSlot == null || (targetSlot is ActionBarSlot && IsItemAlreadyInActionBar())){
            ResetToOriginalSlot();
            return;
        }
        originalSlot.ClearCurrentItem();
        canvasGroup.blocksRaycasts = true;
    }

    private bool IsItemAlreadyInActionBar()
    {
        foreach (var slot in FindObjectsOfType<ActionBarSlot>())
        {
            if (slot.currentItem != null && slot.currentItem.GetComponent<ItemDataHandler>().GetItem == GetComponent<ItemDataHandler>().GetItem)
            {
                return true;
            }
        }
        return false;
    }

    public void ResetToOriginalSlot()
    {
        transform.SetParent(originalSlot.transform);
        originalSlot.AssignCurrentItem(this.gameObject);
        transform.localPosition = Vector2.zero;
        canvasGroup.blocksRaycasts = true;
    }

    void OnDestroy()
    {
        originalSlot.ClearCurrentItem();
    }
}