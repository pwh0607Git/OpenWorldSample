using CustomInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//IPointerClickHandler,
public class ItemIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HorizontalLine("Icon Data"), HideField] public bool s1; 
    public DragAndDropSlot originalSlot;
    public Item item {get; private set;}
    [HorizontalLine(""), HideField] public bool e1;
    [Space(10)]
    [HorizontalLine("UI Conponent"), HideField] public bool s2; 
    private RectTransform rectTransform;
    private Image iconImage;  // 아이콘 이미지 추가
    [SerializeField] TextMeshProUGUI itemCount;
    private CanvasGroup canvasGroup;
    [HorizontalLine(""), HideField] public bool e2;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        iconImage = GetComponent<Image>();
        canvasGroup.blocksRaycasts = true;
    }

    public void Initialize(Item item){
        this.item = item;
        if (item is Consumable consumable)
            consumable.SubscribeToUseEvent(UpdateUI);

        SetItemIcon();
        UpdateUI();
    }

    private void SetItemIcon()
    {
        if (item != null && item.data.icon != null)
        {
            iconImage.sprite = item.data.icon;  // 아이템 데이터의 아이콘 적용
        }
    }

    public void UpdateUI()
    {
        if (item != null)
            itemCount.text = item.count.ToString();
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