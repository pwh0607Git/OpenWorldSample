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

    public PlayerStateView playerStateView;                 // Right Component
    public PlayerHealthbarView playerHealthbarView;
    private PlayerStatePresenter playerStatePresenter;
    
    public EquipmentView equipmentView;                     // Left Component
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
    
        //PlayerData
        PlayerStateModel playerStateModel = new PlayerStateModel();
        playerStatePresenter = new PlayerStatePresenter(playerStateModel, playerStateView, playerHealthbarView);

        //Equipment
        EquipmentModel equipmentModel = new EquipmentModel();
        equipmentPresenter = new EquipmentPresenter(equipmentModel, playerStateModel, equipmentView);
        
        playerDataPresenter = new PlayerDataPresenter(playerStatePresenter, equipmentPresenter);
        
        //item 효과 적용 옵저버.
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
        Debug.Log($"Inventory Init! datasCount : {datas}");
        inventoryPresenter.InitModel(datas);
    }
    public void GetItem(ItemData item){
        Debug.Log($"PlayUIPresenter : GetItem - {item}");
        inventoryPresenter.AddItem(item);
    }
    #endregion

    #region Actionbar
    public void InitActionbar(List<SlotData<KeyCode>> slotDatas){
        Debug.Log("Actionbar Init!");
        Dictionary<KeyCode, Item> datas = new();
        foreach(var data in slotDatas){
            if(data.item == null) datas[data.slotKey] = null;
            else datas[data.slotKey] = inventoryPresenter.GetItemInstance(data.item.data);
        }
        actionbarPresenter.InitModel(datas);
    }
    #endregion
    
    
    #region State

    #endregion
    
    #region Tester
    public Dictionary<int, Item> GetInventoryModel(){
        return inventoryPresenter.GetList();
    }

    public Dictionary<KeyCode, Item> GetActionbarModel(){
        return actionbarPresenter.GetList();
    }
    #endregion

    public void ApplyEffect(IStateEffect effect){
        Debug.Log($"Effect : {effect} 적용하기!");
        // playerStatePresenter.ApplyEffect(effect);/
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
        Debug.Log($"playerDataPresenter : {playerDataPresenter}");
        yield return new WaitUntil(() => playerDataPresenter != null);
        Debug.Log("InitPlayerData");
        playerDataPresenter.SerializePlayerData(datas);
    }
    #endregion
}