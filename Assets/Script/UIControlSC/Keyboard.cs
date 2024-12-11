using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Keyboard : MonoBehaviour
{
    public int maxSlotSize = 8;
    public GameObject slotPrefab;
    public List<KeyboardSlot> slots;
    public Transform slotParent;
    public KeyCode[] keyCodes = new KeyCode[8];

    private void Start()
    {
        CreateKeyboardSlot();
    }

    void CreateKeyboardSlot()
    {
        float spacingX = 0f;

        Vector2 startPosition = new Vector2(-300f, 45f);
        Vector2 componentSize = new Vector2(85f, 85f);

        for (int i = 0; i < maxSlotSize; i++)
        {
            GameObject slotInstance = Instantiate(slotPrefab);
            slotInstance.transform.SetParent(slotParent);
            slotInstance.GetComponent<KeyboardSlot>().SetAssigneKey(keyCodes[i]);
            slots.Add(slotInstance.GetComponent<KeyboardSlot>());

            RectTransform rectTransform = slotInstance.GetComponent<RectTransform>();
            rectTransform.sizeDelta = componentSize;
            rectTransform.anchoredPosition = new Vector2(startPosition.x + i * (componentSize.x + spacingX), startPosition.y);
            rectTransform.localScale = Vector2.one;
        }
    }

    /*
        itemType = Consumable, Equipment, ETC,
        subType = ConsumableType, EquipmentType
    */
    public bool SearchItemByType<T>(ItemType itemType, T? subType = null) where T : struct
    {
        bool res = false;
        //slot들에 루프를 돌아서 존재하는 지 찾기
        foreach (KeyboardSlot slot in slots)
        {
            //칸은 비어있고, 키보드에 해당 아이템이 존재하지 않다면 false.
            if (slot.GetCurrentItem() == null) continue;
            
            ItemData slotItemData = slot.GetCurrentItem().GetComponent<ItemDataSC>().GetItem;

            Debug.Log($"current Slot Item Data : {slotItemData}");

            if (slotItemData != null && slotItemData.itemType == itemType)
            {
                Debug.Log($"current Slot Item Data와 일치!! {slotItemData.itemType} == {itemType}");
                switch (itemType)
                {
                    case ItemType.Consumable:
                        Consumable consumable = (Consumable)slotItemData;
                        Debug.Log($"current Slot Item sub type Data {consumable.subType} == {subType}");
                        if (consumable != null && consumable.subType.Equals(subType))
                        {
                            return true;
                        }
                    break;
                }
            }
        }
        return false;
    }

    //keyboarslot에서만 호출하기.
    public void UpdateKeyboardPreset()
    {
        foreach(KeyboardSlot slot in slots)
        {
            if(slot.gameObject.transform.childCount == 0) continue;
            else
            {
                slot.SetCurrentItem(slot.gameObject.transform.GetChild(0).gameObject);
            }
        }
    }
}