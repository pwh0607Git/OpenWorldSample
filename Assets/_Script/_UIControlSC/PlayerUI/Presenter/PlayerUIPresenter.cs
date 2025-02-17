using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIPresenter : MonoBehaviour
{
    [Header("MVP - Component")]
    public InventoryView inventoryView;
    private InventoryPresenter inventoryPresenter;
    
    [Space(10)]
    
    [Header("Props")]
    [SerializeField] int maxSlotSize;
    private void Start()
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
}
