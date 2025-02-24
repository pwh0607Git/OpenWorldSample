using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableItemHandler : ItemDataHandler
{
    [SerializeField] Consumable consumableItem;

    public override ItemData GetItem => consumableItem;

    private Image iconImg;

    public TextMeshProUGUI consumableCountText;

    public override void Init(ItemData itemData){
        consumableItem = (Consumable)itemData;
        iconImg = GetComponent<Image>();
        consumableCountText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        if (consumableItem != null)
        {
            consumableItem.OnConsumableUsed += UpdateCountCallback;
        }
        UpdateCountCallback();
        MapImage();
        SetCount();
        //text 매핑도 필요.
    }

    void SetCount(){
        consumableCountText.text = consumableItem.GetConsumableCount().ToString();
    }

    void OnDestroy()
    {
        if (consumableItem != null)
        {
            consumableItem.OnConsumableUsed -= UpdateCountCallback;
        }
    }
    private void MapImage()
    {
        if (iconImg == null)
            iconImg = GetComponent<Image>();

        if (consumableItem != null && iconImg != null)
        {
            iconImg.sprite = consumableItem.icon;
        }
    }

    private void UpdateCountCallback()
    {
        if (consumableItem == null) Destroy(gameObject);

        int consumableCount = consumableItem.GetConsumableCount();

        if (consumableCount <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            consumableCountText.text = consumableCount.ToString();
        }
    }
}