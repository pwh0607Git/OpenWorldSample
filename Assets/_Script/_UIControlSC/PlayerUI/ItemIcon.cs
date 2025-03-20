using System.Collections;
using CustomInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//IPointerClickHandler,
public class ItemIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HorizontalLine("Icon Data"), HideField] public bool s1; 
    public DragAndDropSlot originalSlot;
    public Item item { get; private set; }
    [HorizontalLine(""), HideField] public bool e1;
    [Space(10)]
    [HorizontalLine("UI Component"), HideField] public bool s2;
    [SerializeField] PlayerUIPresenter presenter; 
    private RectTransform rectTransform;
    private Image iconImage;                        // 아이콘 이미지 추가
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

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GetComponentInParent<PlayerUIPresenter>() != null);
        presenter = GetComponentInParent<PlayerUIPresenter>();
    }

    public void Initialize(Item item){
        this.item = item;
        if(iconImage.sprite != null) return;
        
        SetIconImage();
        UpdateIcon();
    }

    private void SetIconImage()
    {
        if (item != null && item.data.icon != null) iconImage.sprite = item.data.icon;              // 아이템 데이터의 아이콘 적용
    }

    public void UpdateIcon()
    {
        if(item.count <= 0){
            Destroy(gameObject);
            transform.GetComponentInParent<DragAndDropSlot>().ClearSlot(true);          //true를 보내어 model을 갱신할 것.
        }

        if (item != null) itemCount.text = item.count.ToString();
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        presenter.ShowItemPopUp(item.data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        presenter.HideItemPopUp();
    }

    public void ResetToOriginalSlot(){
        gameObject.transform.SetParent(originalSlot.transform);
        rectTransform.anchoredPosition = Vector2.zero;
    }
}