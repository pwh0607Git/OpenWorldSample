using System.Collections.Generic;
using UnityEngine;

//장비 슬롯은 고정위치.
public class EquipmentModel : MonoBehaviour
{
    private Dictionary<EquipmentType, ItemData> equipedItems = new Dictionary<EquipmentType, ItemData>();

    public EquipmentModel(){

    }

    public bool EquipItem(){
                
        return true;
    }

    public Dictionary<EquipmentType, ItemData> GetEquipmentItems(){
        return new Dictionary<EquipmentType, ItemData>(equipedItems);
    }
}