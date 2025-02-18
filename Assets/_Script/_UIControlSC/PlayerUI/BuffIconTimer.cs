using System;
using UnityEngine;
using UnityEngine.UI;

public class BuffIconTimer : MonoBehaviour
{
    private float curTime;
    private float duration;
    private bool timerRunning;

    public GameObject buffTimerImg;
    private Image buffStateBar;

    public Action<GameObject> OnBuffEnd;        // ���� ���� �� ������ �ݹ�

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