using System.Collections.Generic;
using UnityEngine;

public class ActionbarPresenter
{
    private ActionbarModel model;
    private ActionbarView view;

    public ActionbarPresenter(ActionbarModel model, ActionbarView view){
        this.model = model;
        this.view = view;
        view.OnViewUpdated += UpdateModelDataFromView;
        model.OnModelUpdated += UpdateViewFromModel;
    }

    // 다음 진행.
    public void InitModel(List<SlotData<KeyCode>> slotDatas){
        Debug.Log($"{GetType()} - Init bar code : {slotDatas.Count}");
        model.InitModel(slotDatas);
    }

    // Model -> View
    public void UpdateViewFromModel(){
        Debug.Log($"{GetType()} - Update View");
        view.UpdateView(model.GetSlotDatas());
    }

    // View -> Model
    public void UpdateModelDataFromView(SlotData<KeyCode> slot){
        Debug.Log($"{GetType()} - Update Model {slot.slotKey} : {slot.itemData}");
        model.UpdateModelDataFromView(slot);
    } 

    #region Inspector Caller
    public Dictionary<KeyCode, Item> GetList() => model.GetSlotDatas();
    #endregion
}