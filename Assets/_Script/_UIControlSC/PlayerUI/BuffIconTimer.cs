using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffIconTimer : MonoBehaviour
{
    private float curTime;
    private float duration;
    private bool timerRunning;
    public GameObject buffTimerImg;
    private Image buffDurationBar;
    public IStateEffect effect {get; private set;}
    public Action<BuffIconTimer> OnBuffEnd; 

    private void Start()
    {
        buffDurationBar = buffTimerImg.GetComponent<Image>();
    }

    public void InitBuff(IStateEffect effect){
        this.effect = effect;
        GetComponent<Image>().sprite = effect.Data.icon;
        StartTimer(effect.Data.duration);
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