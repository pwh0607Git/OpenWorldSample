using System.Collections.Generic;
using UnityEngine;

public class ActionbarModel : MonoBehaviour
{
    public int maxSlotSize {get; private set;}
    public List<KeyCode> keyCodes = new List<KeyCode>();
    public List<ActionBarSlotComponent> slots;
    public ActionbarModel()
    {
        // this.maxSlotSize = maxSlotSize;
        // this.keyCodes = keyCodes;
        // slots = new List<ActionBarSlotComponent>();
    }

    public List<KeyCode> GetKeyCodes(){
        return new List<KeyCode>(keyCodes);
    }
    
    public void Serialize(List<KeyCode> codes){
        this.keyCodes = codes;
    }
}