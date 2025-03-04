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
        Debug.Log($"Actionbar Presenter - Init bar code : {slotDatas.Count}");
        model.InitModel(slotDatas);
    }

    // Model -> View
    public void UpdateViewFromModel(){
        Debug.Log($"Actionbar Presenter : Update View");
        view.UpdateView(model.GetSlotDatas());
    }

    // View -> Model
    public void UpdateModelDataFromView(SlotData<KeyCode> slot){
        
        Debug.Log($"Actionbar Presenter : Update Model {slot.slotKey} : {slot.item}");
        model.UpdateModelDataFromView(slot);
    } 

    #region Inspector Caller
    public Dictionary<KeyCode, ItemData> GetList() => model.GetSlotDatas();
    #endregion
}