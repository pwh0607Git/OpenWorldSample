using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPopup : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI description;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetItemData(ItemData itemData){
        itemIcon.sprite = itemData.icon;
        itemName.text = itemData.name;
        description.text = itemData.description;
    }
}
