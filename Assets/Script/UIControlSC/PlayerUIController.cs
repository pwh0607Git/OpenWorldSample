using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerUIController : MonoBehaviour
{
    public GameObject inventoryWindow;
    public  GameObject equipmentWindow;

    //Stack<GameObject> activeWindows;

    private void Start()
    {
        //activeWindows = new Stack<GameObject>(); // 스택 초기화
        inventoryWindow.SetActive(false);
        equipmentWindow.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryWindow.activeSelf)
            {
                inventoryWindow.SetActive(false);
            }
            else {
                inventoryWindow.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (equipmentWindow.activeSelf)
            {
                equipmentWindow.SetActive(false);
            }
            else
            {
                equipmentWindow.SetActive(true);
            }
        }

        /*
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetWindowsActiveEvent();
        }
        */
    }
}