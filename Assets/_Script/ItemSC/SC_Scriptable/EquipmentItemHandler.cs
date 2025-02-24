using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentItemHandler : ItemDataHandler
{
    [SerializeField]
    private Equipment equipmentItem;

    public override ItemData GetItem => equipmentItem;

    private Image iconImg;
    
    public override void Init(ItemData itemData)
    {
        iconImg = GetComponent<Image>();
        equipmentItem = (Equipment)itemData;
        MapImage();
        Destroy(GetComponentInChildren<TextMeshProUGUI>().gameObject);          //text는 삭제!
    }

    private void MapImage()
    {
        if (iconImg == null)
            iconImg = GetComponent<Image>();

        if (equipmentItem != null && iconImg != null)
        {
            iconImg.sprite = equipmentItem.icon;           // ������ ����
        }
    }
}