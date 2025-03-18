using System.Collections;
using System.Collections.Generic;
using System.Data;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUIPresenter : MonoBehaviour
{
    [Header("MVP - Component")]
    public InventoryView inventoryView;
    private InventoryPresenter inventoryPresenter;

    public ActionbarView actionBarView;
    private ActionbarPresenter actionbarPresenter; 

    public PlayerStateView playerStateView;           // Right Component
    public PlayerHealthbarView playerHealthbarView;
    private PlayerDataPresenter playerDataPresenter;
    
    public EquipmentView equipmentView;             // Left Component
    private EquipmentPresenter equipmentPresenter;

    [SerializeField] ItemInfoPopup popup;

    [Space(10)]
    [Header("Initial Data")]
    [SerializeField] int maxSlotSize;
    private object playerStatePresenter;

    IEnumerator Start()
    {
        InventoryModel inventoryModel = new InventoryModel(maxSlotSize);
        inventoryPresenter = new InventoryPresenter(inventoryModel, inventoryView);

        ActionbarModel actionbarModel = new ActionbarModel();
        actionbarPresenter = new ActionbarPresenter(actionbarModel, actionBarView);
    
        //PlayerData
        PlayerStateModel playerStateModel = new PlayerStateModel();
        //Equipment
        EquipmentModel equipmentModel = new EquipmentModel();
        
        playerDataPresenter = new PlayerDataPresenter(playerStateModel, playerStateView, playerHealthbarView);
        equipmentPresenter = new EquipmentPresenter(equipmentModel, playerStateModel, equipmentView);
        
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
            if(data.itemData == null) datas[data.slotKey] = null;
            else datas[data.slotKey] = inventoryPresenter.GetItemInstance(data.itemData);
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
    public void InitEquipment(List<SlotData<EquipmentType>> datas){
        List<Equipment> es = new List<Equipment>();

        foreach(var data in datas){
            Equipment item = new Equipment((EquipmentData)data.itemData);
            es.Add(item);
        }

        StartCoroutine(TestFunc(es));
        StartCoroutine(TestFunc2());
    }

    IEnumerator TestFunc(List<Equipment> es){
        yield return new WaitUntil(() => playerDataPresenter != null);
        playerDataPresenter.SerializeModel(es);
    }

    IEnumerator TestFunc2(){        
        yield return new WaitUntil(() => equipmentPresenter != null);
        equipmentPresenter.UpdateViewFromModel();
    }
    #endregion
}