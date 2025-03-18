using System.Collections.Generic;
using UnityEngine;

public class EquipmentTester : MonoBehaviour
{
    
    [Header("Reference")]
    public PlayerUIPresenter uiPresenter;

    [SerializeField] List<SlotData<EquipmentType>> datas;

    void Start()
    {
        SendEquipmentData();
    }

    public void SendEquipmentData(){
        uiPresenter.SerializeEquipment(datas);
    }
}
