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
        model.InitModel(slotDatas);
    }

    public void UpdateView(){
        view.UpdateView(model.GetSlotDatas());
    }

    public void UpdateModel(SlotData<KeyCode> slot){
        model.UpdateModel(slot);
    } 

    #region Inspector Caller
    public Dictionary<KeyCode, Item> GetList() => model.GetSlotDatas();
    #endregion
}