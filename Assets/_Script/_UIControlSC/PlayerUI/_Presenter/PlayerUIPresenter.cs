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
    private PlayerDataPresenter playerDataPresenter;

    public PlayerStateView playerStateView;
    public PlayerHealthbarView playerHealthbarView;
    public BuffStateView buffStateView;
    private PlayerStatePresenter playerStatePresenter;
    
    public EquipmentView equipmentView;
    private EquipmentPresenter equipmentPresenter;

    [SerializeField] ItemInfoPopup popup;

    [Space(10)]
    [Header("Initial Data")]
    [SerializeField] int maxSlotSize;

    IEnumerator Start()
    {
        InventoryModel inventoryModel = new InventoryModel(maxSlotSize);
        inventoryPresenter = new InventoryPresenter(inventoryModel, inventoryView);

        ActionbarModel actionbarModel = new ActionbarModel();
        actionbarPresenter = new ActionbarPresenter(actionbarModel, actionBarView);
    
        PlayerStateModel playerStateModel = new PlayerStateModel();
        playerStatePresenter = new PlayerStatePresenter(playerStateModel, playerStateView, playerHealthbarView, buffStateView);

        EquipmentModel equipmentModel = new EquipmentModel();
        equipmentPresenter = new EquipmentPresenter(equipmentModel, playerStateModel, equipmentView);
        playerDataPresenter = new PlayerDataPresenter(playerStatePresenter, equipmentPresenter);
        
        yield return new WaitUntil(() => ItemUsedManager.Instance != null);
        ItemUsedManager.Instance.OnItemUsed += ApplyEffect;
    }  

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            inventoryPresenter.ToggleInventory();
        if(Input.GetKeyDown(KeyCode.E))
            playerDataPresenter.TogglePlayerDataView();
    }

    #region Inventory
    public void InitInventory(List<SlotData<int>> datas){
        inventoryPresenter.InitModel(datas);
    }
    public void GetItem(ItemData item){
        inventoryPresenter.AddItem(item);
    }
    #endregion

    #region Actionbar
    public void InitActionbar(List<SlotData<KeyCode>> slotDatas){
        Dictionary<KeyCode, Item> datas = new();
        foreach(var data in slotDatas){
            if(data.item == null) datas[data.slotKey] = null;
            else datas[data.slotKey] = inventoryPresenter.GetItemInstance(data.item.data);
        }
        actionbarPresenter.InitModel(datas);
    }
    #endregion
    
    #region Tester
    public Dictionary<int, Item> GetInventoryModel(){
        return inventoryPresenter.GetList();
    }

    public Dictionary<KeyCode, Item> GetActionbarModel(){
        return actionbarPresenter.GetList();
    }
    
    public PlayerState GetPlayerState(){
        return playerStatePresenter.GetPlayerState();
    }
    #endregion

    public void ApplyEffect(IStateEffect effect){
        playerStatePresenter.ApplyEffect(effect);
    }

    #region Icon Event
    public void ShowItemPopUp(ItemData itemData){
        popup.gameObject.SetActive(true);
        popup.SetItemData(itemData);
    }

    public void HideItemPopUp(){
        popup.gameObject.SetActive(false);
    }
    #endregion

    #region Equipments
    public void SerializeEquipment(List<SlotData<EquipmentType>> datas){
        List<Equipment> es = new List<Equipment>();
        StartCoroutine(InitPlayerData(datas));
    }

    IEnumerator InitPlayerData(List<SlotData<EquipmentType>> datas){
        yield return new WaitUntil(() => playerDataPresenter != null);
        playerDataPresenter.SerializePlayerData(datas);
    }
    #endregion
}