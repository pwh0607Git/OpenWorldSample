using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//IPointerClickHandler,
public class ItemIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public DragAndDropSlot originalSlot;
    private Image iconImage;  // 아이콘 이미지 추가
    public Item item {get; private set;}
    [SerializeField] TextMeshProUGUI itemCount;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        iconImage = GetComponent<Image>();
        canvasGroup.blocksRaycasts = true;
    }

    // 외부로 부터 주입받기
    public void Initialize(Item item){
        this.item = item;
        if (item is Consumable consumable)
        {
            consumable.SubscribeToUseEvent(UpdateUI);
        }
        SetItemIcon();
        UpdateUI();
    }

    private void SetItemIcon()
    {
        if (item != null && item.GetData.icon != null)
        {
            iconImage.sprite = item.GetData.icon;  // 아이템 데이터의 아이콘 적용
        }
    }

    public void UpdateUI()
    {
        if (item != null)
            itemCount.text = item.Count.ToString();
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