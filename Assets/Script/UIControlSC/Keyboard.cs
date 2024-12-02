using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Keyboard : MonoBehaviour
{
    public static Keyboard myKeyboard { get; private set; }

    private void Awake()
    {
        if (myKeyboard == null)
        {
            myKeyboard = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int maxSlotSize = 8;
    public GameObject slotPrefab;
    public List<KeyboardSlot> slots;            //Dictionary<KeyboardSlot, int>... 로 변경 예정.
    public Transform slotParent;

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
            slotInstance.GetComponent<KeyboardSlot>().assignedKey = KeyCode.Alpha1 + i;
            RectTransform rectTransform = slotInstance.GetComponent<RectTransform>();
            rectTransform.sizeDelta = componentSize;
            rectTransform.anchoredPosition = new Vector2(startPosition.x + i * (componentSize.x + spacingX), startPosition.y);
        }
    }

    private void AddKeyboardSlotRef(KeyboardSlot slotRef)
    {
        slots.Add(slotRef);
    }

    /*
    public bool searchSlotItem (ItemData itemData)
    {

    }
    */
}
