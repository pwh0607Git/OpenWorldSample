using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentItemSC : ItemDataSC
{
    [SerializeField]
    private Equipment equipmentItem;

    public override ItemData GetItem => equipmentItem;

    private Image iconImg;
    
    private void Start()
    {
        iconImg = GetComponent<Image>();
    }

    public void SetItem(Equipment itemData)
    {
        equipmentItem = itemData;

        MapImage();
    }

    private void MapImage()
    {
        if (iconImg == null)
            iconImg = GetComponent<Image>();

        if (equipmentItem != null && iconImg != null)
        {
            iconImg.sprite = equipmentItem.icon;           // 아이콘 설정
        }
    }
}