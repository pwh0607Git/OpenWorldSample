using System.Collections.Generic;
using UnityEngine;

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
    public void InitInventory(List<SlotData<int>> datas){
        Debug.Log($"Inventory Init! datasCount : {datas}");
        inventoryPresenter.InitModel(datas);
    }

    public void GetItem(ItemData item){
        Debug.Log($"PlayUIPresenter : GetItem - {item}");
        inventoryPresenter.AddItem(item);
    }

    public void SerializeActionbar(List<ActionBarSlotComponent> components){
        Debug.Log("Actionbar Init!");
        actionbarPresenter.SerializeModel(components);
    }

    public void SerializePlayerState(){
        // playerStatePresenter.se
    }
    public Dictionary<int, ItemData> GetInventoryModel(){
        return inventoryPresenter.GetList();
    }

    public List<ActionBarSlotComponent> ShowActionbarModel(){
        return actionbarPresenter.GetList();
    }
}
