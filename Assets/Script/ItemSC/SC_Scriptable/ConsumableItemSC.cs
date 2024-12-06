using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//실질적 임무수행 코드.
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

        MapImage();         // 데이터 설정 후 아이콘 매핑                    // 데이터 설정 후 아이콘 매핑
    }

    private void MapImage()
    {
        if (iconImg == null)
            iconImg = GetComponent<Image>();

        if (consumableItem != null && iconImg != null)
        {
            iconImg.sprite = consumableItem.icon;           // 아이콘 설정
        }
    }

    //콜백용.
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
            Debug.Log($"TEXT : {consumableCount}");
        }
    }
}