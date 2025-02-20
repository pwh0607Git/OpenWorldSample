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
}