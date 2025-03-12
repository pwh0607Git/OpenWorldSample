using System;
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

    [SerializeField] ItemInfoPopup popup;

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

        ItemUsedManager.Instance.OnItemUsed += ApplyEffect;
    }  

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPresenter.ToggleInventory();
        }
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
            if(data.itemData == null) datas[data.slotKey] = null;
            else datas[data.slotKey] = inventoryPresenter.GetItemInstance(data.itemData);
        }
        actionbarPresenter.InitModel(datas);
    }
    #endregion
    
    
    #region State
    public void SerializePlayerState(){
        
    }

    public void TakeDamage(int damage){
        playerStatePresenter.TakeDamage(damage);
    }
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
}