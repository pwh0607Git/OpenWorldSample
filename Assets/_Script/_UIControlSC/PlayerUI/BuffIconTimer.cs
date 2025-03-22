using System;
using UnityEngine;
using UnityEngine.UI;

public class BuffIconTimer : MonoBehaviour
{
    private float curTime;
    private float duration;
    private bool timerRunning;

    public GameObject buffTimerImg;
    private Image buffDurationBar;


    public Action<BuffIconTimer> OnBuffEnd; 

    private void Start()
    {
        buffDurationBar = buffTimerImg.GetComponent<Image>();
    }

    public void InitBuff(ItemData data){
        GetComponent<Image>().sprite = data.icon;
        StartTimer(((BuffConsumableData)data).duration);
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
        buffDurationBar.fillAmount = curTime / duration;
    }

    private void OnDestroy()
    {
        OnBuffEnd?.Invoke(this);
    }
}