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
    public float duration;
    private bool timerRunning;

    private bool blinking;
    public GameObject buffTimerImg;
    private Image buffStateBar;

    private BuffManager buffManager;

    public Action<GameObject> OnBuffEnd; // 버프 종료 시 실행할 콜백

    private void Start()
    {
        buffStateBar = buffTimerImg.GetComponent<Image>();
        BuffManager buffManager = FindObjectOfType<BuffManager>();
        StartTimer();
    }

    public void StartTimer()
    {
        timerRunning = true;
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
