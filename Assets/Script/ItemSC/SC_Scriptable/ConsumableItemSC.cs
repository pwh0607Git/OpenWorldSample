using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableItemSC : MonoBehaviour
{
    [SerializeField]
    private Consumable consumableItem;  // 연결된 Consumable 아이템

    public Consumable GetItem
    {
        get => consumableItem;
        set
        {
            consumableItem = value;
            MapImage();                 //아이템이 설정될 때 이미지를 매핑
        }
    }

    private Image iconImg;

    private void Start()
    {
        iconImg = GetComponent<Image>();
    }

    // 아이템의 아이콘을 이미지에 매핑하는 함수
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
