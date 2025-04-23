using UnityEngine;

public class SidebarSlot : MonoBehaviour
{
    public GameObject item;
    [SerializeField] GameObject focusFrame;

    void Start(){
        item = null;
    }
    
    public void FocusSlot(bool on){
        focusFrame.SetActive(on);
    }
}
