using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIPresenter : MonoBehaviour
{
    [Header("MVP - Component")]
    public InventoryView inventoryView;
    private InventoryPresenter inventoryPresenter;

    public ActionbarView actionBarView;
    private ActionbarPresenter actionbarPresenter; 
    
    [Space(10)]
    [Header("Props")]
    [SerializeField] int maxSlotSize;
    void Start()
    {
        InventoryModel model = new InventoryModel(maxSlotSize);
        inventoryPresenter = new InventoryPresenter(model, inventoryView);
    }  

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPresenter.ToggleInventory();
        }
    }

    //Test
    public void SerializeInventory(List<ItemEntry> entries){
        // inventoryPresenter에게 serList에 맞게 InventoryModel을 수정해 줄 것을 요구한다.
        Debug.Log("Inventory Init!");
        inventoryPresenter.SerializeModel(entries);
    }

    public void GetItem(ItemData item){
        inventoryPresenter.AddItem(item);
    }
}
