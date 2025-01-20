using TMPro;
using UnityEngine;
using UnityEngine.UI;

//������ �ӹ����� �ڵ�.
public class ConsumableItemSC : ItemDataSC
{
    [SerializeField]
    private Consumable consumableItem;

    public override ItemData GetItem => consumableItem;

    private Image iconImg;

    public TextMeshProUGUI consumableCountText;

    private void Start()
    {
        iconImg = GetComponent<Image>();
        consumableCountText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        if(consumableCountText == null)
        {
            Debug.LogWarning("CountText �Ҵ� ����...");
        }

        UpdateCountCallback();

        if (consumableItem != null)
        {
            consumableItem.OnConsumableUsed += UpdateCountCallback;
        }
    }
    
    public void SetItem(Consumable itemData)
    {
        if (consumableItem != null)
        {
            consumableItem.OnConsumableUsed -= UpdateCountCallback;
        }

        consumableItem = itemData;

        if (consumableItem != null)
        {
            consumableItem.OnConsumableUsed += UpdateCountCallback;
        }

        MapImage();         // ������ ���� �� ������ ����                    // ������ ���� �� ������ ����
    }

    private void MapImage()
    {
        if (iconImg == null)
            iconImg = GetComponent<Image>();

        if (consumableItem != null && iconImg != null)
        {
            iconImg.sprite = consumableItem.icon;           // ������ ����
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