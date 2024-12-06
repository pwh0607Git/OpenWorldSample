using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWindow : MonoBehaviour
{
    public static EquipmentWindow myEquipments { get; private set; }

    private void Awake()
    {
        if (myEquipments == null)
        {
            myEquipments = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }
}
