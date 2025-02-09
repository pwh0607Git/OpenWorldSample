using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffIconTimer : MonoBehaviour
{
    private float curTime;
    private float duration;
    private bool timerRunning;

    public GameObject buffTimerImg;
    private Image buffStateBar;

    public Action<GameObject> OnBuffEnd;        // 버프 종료 시 실행할 콜백

    private void Start()
    {
        buffStateBar = buffTimerImg.GetComponent<Image>();
    }

    public void StartTimer(float duration)
    {
        timerRunning = true;
        this.duration = duration;
        curTime = 0;
    }

    private void Update()
    {
        curTime += Time.deltaTime;
        if (curTime >= duration)
        {
            timerRunning = false;
            Destroy(gameObject);
        }
        UpdateBuffState();
    }

    private void UpdateBuffState()
    {
        buffStateBar.fillAmount = curTime / duration;
    }

    private void OnDestroy()
    {
        OnBuffEnd?.Invoke(gameObject);
    }
}