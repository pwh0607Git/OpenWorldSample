using UnityEngine;

public class ActionbarPresenter : MonoBehaviour
{
    private ActionbarModel model;
    private ActionbarView view;

    public ActionbarPresenter(ActionbarModel model, ActionbarView view){
        this.model = model;
        this.view = view;
        CreateSlots();
    }

    //view에 표기하기 !!!
    private void CreateSlots(){
        for(int i=0;i<model.maxSlotSize;i++){
            ActionBarSlotComponent slot = new ActionBarSlotComponent(model.keyCodes[i]);
            model.slots.Add(slot);
            view.CreateSlotView(slot);

            // 위치 설정
            // RectTransform rectTransform = slot.GetComponent<RectTransform>();
            // rectTransform.sizeDelta = componentSize;
            // rectTransform.anchoredPosition = new Vector2(startPosition.x + i * (componentSize.x + spacingX), startPosition.y);
            // rectTransform.localScale = Vector2.one;
        }
    }
}