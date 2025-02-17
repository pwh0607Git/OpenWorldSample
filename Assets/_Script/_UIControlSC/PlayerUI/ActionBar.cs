using System.Collections.Generic;
using UnityEngine;

public class ActionBar : MonoBehaviour
{
    public int maxSlotSize = 8;
    public GameObject slotPrefab;
    public List<ActionBarSlot> slots;
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
            GameObject slotInstance = Instantiate(slotPrefab, slotParent);
            slotInstance.GetComponent<ActionBarSlot>().SetAssigneKey(keyCodes[i]);
            slots.Add(slotInstance.GetComponent<ActionBarSlot>());

            RectTransform rectTransform = slotInstance.GetComponent<RectTransform>();
            rectTransform.sizeDelta = componentSize;
            rectTransform.anchoredPosition = new Vector2(startPosition.x + i * (componentSize.x + spacingX), startPosition.y);
            rectTransform.localScale = Vector2.one;
        }
    }
}