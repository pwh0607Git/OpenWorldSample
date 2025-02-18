using UnityEngine;

public class EquipmentPresenter
{
    public EquipmentModel model;
    public EquipmentView view;

    public EquipmentPresenter(EquipmentModel model, EquipmentView view){
        this.model = model;
        this.view = view;
    }

    public Transform originalParent;
}