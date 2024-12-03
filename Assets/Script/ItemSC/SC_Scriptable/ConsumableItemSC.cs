using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableItemSC : ItemDataSC
{
    [SerializeField]
    private Consumable consumableItem;

    public override ItemData GetItem => consumableItem;

    private Image iconImg;

    private void Start()
    {
        iconImg = GetComponent<Image>();
    }
    
    public void SetItem(Consumable itemData)
    {
        consumableItem = itemData;      // 데이터 설정
        MapImage();                     // 데이터 설정 후 아이콘 매핑
    }

    public void MapImage()
    {
        if (iconImg == null)
            iconImg = GetComponent<Image>();

        if (consumableItem != null && iconImg != null)
        {
            iconImg.sprite = consumableItem.icon;  // 아이콘 설정
        }
        else
        {
            Debug.LogError("아이콘 또는 Consumable 아이템이 설정되지 않았습니다.");
        }
    }
}
