using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    public GameObject inventory;
    public GameObject equipment;

    Stack<GameObject> activeWindows;        //활성화 되어있는 창들..

    private void Start()
    {
        inventory.SetActive(false);
        equipment.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SetSingleWindowActive(inventory);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SetSingleWindowActive(equipment);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetWindowsActiveEvent();
        }
    }

    private void SetSingleWindowActive(GameObject window)
    {   
        window.SetActive(!window.activeSelf);
        activeWindows.Push(window);
    }

    private void SetWindowsActiveEvent()
    {
        if(activeWindows.Count == 0)
        {
            Debug.Log("현재 열여 있는 윈도우가 존재하지 않습니다.");
            return;
        }

        while (activeWindows.Count > 0)
        {
            GameObject windowRef = activeWindows.Pop();

            if (windowRef.activeSelf)
            {
                //윈도우가 활성화된 상태라면 닫기
                windowRef.SetActive(false);
                break;
            }
        }
    }
}
