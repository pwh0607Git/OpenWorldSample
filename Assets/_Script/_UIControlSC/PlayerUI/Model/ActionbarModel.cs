using System.Collections.Generic;
using UnityEngine;

public class ActionbarModel : MonoBehaviour
{
    public int maxSlotSize {get; private set;}
    public KeyCode[] keyCodes = new KeyCode[8];
    public List<ActionBarSlotComponent> slots;
    public ActionbarModel(int maxSlotSize, KeyCode[] keyCodes)
    {
        this.maxSlotSize = maxSlotSize;
        this.keyCodes = keyCodes;
        slots = new List<ActionBarSlotComponent>();
    }
}