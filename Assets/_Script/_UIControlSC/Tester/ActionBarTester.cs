using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionBarTester : MonoBehaviour
{
    public List<KeyCode> codes = new List<KeyCode>{KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H};

    public PlayerUIPresenter uiPresenter;
    
    public void SendCodes(){
        uiPresenter.SerializeActionbar(codes);
    }
}