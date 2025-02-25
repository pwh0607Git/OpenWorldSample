using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerUIPresenter : MonoBehaviour
{
    [Header("MVP - Component")]
    public InventoryView inventoryView;
    private InventoryPresenter inventoryPresenter;

    public ActionbarView actionBarView;
    private ActionbarPresenter actionbarPresenter; 

    public PlayerStateView playerStateView;
    private PlayerStatePresenter playerStatePresenter;

    [Space(10)]
    [Header("Initial Data")]
    [SerializeField] int maxSlotSize;
    void Start()
    {
        InventoryModel inventoryModel = new InventoryModel(maxSlotSize);
        inventoryPresenter = new InventoryPresenter(inventoryModel, inventoryView);

        // StartCoroutine(InitMVP());
        ActionbarModel actionbarModel = new ActionbarModel();
        actionbarPresenter = new ActionbarPresenter(actionbarModel, actionBarView);
    
        PlayerStateModel playerStateModel = new PlayerStateModel();
        playerStatePresenter = new PlayerStatePresenter(playerStateModel, playerStateView);
    }  

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPresenter.ToggleInventory();
        }
    }

    // 초기화용 코드.
    public void SerializeInventory(List<ItemEntry> entries){
        // inventoryPresenter에게 serList에 맞게 InventoryModel을 수정해 줄 것을 요구한다.
        Debug.Log("Inventory Init!");
        inventoryPresenter.SerializeModel(entries);
    }

    public void GetItem(ItemData item){
        inventoryPresenter.AddItem(item);
    }

    public void SerializeActionbar(List<ActionBarSlotComponent> components){
        Debug.Log("Actionbar Init!");
        actionbarPresenter.SerializeModel(components);
    }

    public void SerializePlayerState(){
        // playerStatePresenter.se
    }
}
