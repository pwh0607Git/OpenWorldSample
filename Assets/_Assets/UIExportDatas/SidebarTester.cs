using UnityEngine;
using CustomInspector;

public class SidebarTester : MonoBehaviour
{
    public SidebarController sidebar;
    public Transform parent;
    public GameObject iconItem;

    [Button("MakeItem"), HideField] public bool _btn1;

    // Factory 패턴으로 아이템 아이콘 생성 예정
    void MakeItem(){
        GameObject item = Instantiate(iconItem, parent);
        sidebar.GetItem(item);
    }
}
