using System.Collections.Generic;
using UnityEngine;

public class ActionbarPresenter
{
    private ActionbarModel model;
    private ActionbarView view;

    public ActionbarPresenter(ActionbarModel model, ActionbarView view){
        this.model = model;
        this.view = view;
        view.OnViewUpdated += UpdateModel;
        model.OnModelUpdated += UpdateView;
    }

    // 다음 진행.
    public void InitModel(Dictionary<KeyCode, Item> slotDatas)
    {
        Debug.Log($"{GetType()} - Init bar code : {slotDatas.Count}");
        model.InitModel(slotDatas);
    }

    public void UpdateView(){
        Debug.Log($"{GetType()} - Update View");
        view.UpdateView(model.GetSlotDatas());
    }

    public void UpdateModel(SlotData<KeyCode> slot){
        Debug.Log($"{GetType()} - Update Model {slot.slotKey} : {slot.itemData}");
        model.UpdateModel(slot);
    } 

    #region Inspector Caller
    public Dictionary<KeyCode, Item> GetList() => model.GetSlotDatas();
    #endregion
}