using System.Collections.Generic;
using UnityEngine;

public class ActionbarPresenter
{
    private ActionbarModel model;
    private ActionbarView view;

    public ActionbarPresenter(ActionbarModel model, ActionbarView view){
        this.model = model;
        this.view = view;
        
        if (this.view == null)
        {
            Debug.LogWarning("ActionbarPresenter: View is null! Make sure it is properly initialized.");
        }

        model.OnModelChanged += ModelChangeHandler;
    }

    //view에 표기하기 !!!
    void ModelChangeHandler(){
        Debug.Log($"Action bar Model - InitView code Count :{model.GetComponents().Count}");
        if (view == null)
        {
            Debug.LogWarning("ModelChangeHandler: view is null!");
        }
        view.SerializeSlots(model.GetComponents());
    }

    // 다음 진행.
    public void SerializeModel(List<ActionBarSlotComponent> components){
        Debug.Log($"Action bar Model - Serialize code Count :{components.Count}");
        model.Serialize(components);
    }

    public List<ActionBarSlotComponent> GetList(){
        return model.GetComponents();
    }
}