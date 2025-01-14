using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField]
    public ItemData itemData {get; set;}

    public void CheckType(){
        if(itemData is Consumable){
            Debug.Log($"DataType : Consumable");
        }else if(itemData is Equipment){
            Debug.Log($"DataType : Equipment");
        }
    }
}