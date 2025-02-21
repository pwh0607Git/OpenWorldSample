using System.Collections.Generic;
using UnityEngine;

public class ActionbarPresenter
{
    private ActionbarModel model;
    private ActionbarView view;

    public ActionbarPresenter(ActionbarModel model, ActionbarView view){
        this.model = model;
        this.view = view;
        InitView();
    }

    //view에 표기하기 !!!
    private void InitView(){
        view.CreateSlots(model.GetKeyCodes());
    }

    // 다음 진행.
    public void SerializeModel(List<KeyCode> codes){
        Debug.Log($"Action bar Model - Serialize code Count :{codes.Count}");
        model.Serialize(codes);
    }
}