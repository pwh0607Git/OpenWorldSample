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
    BuffManager buffManager;


    private void Start()
    {
        buffStateBar = buffTimerImg.GetComponent<Image>();
        StartTimer();
    }

    public void StartTimer()
    {
        timerRunning = true;
        duration = 5f;
        curTime = 0;
    }

    private void Update()
    {
        curTime += Time.deltaTime; // 매 프레임 경과 시간 추가
        if (curTime >= duration)
        {
            timerRunning = false;
            Debug.Log("Timer finished!");

            //타이머 종료시 비활성화하기
            Destroy(this);
        }

        UpdateBuffState();
    }

    private void UpdateBuffState()
    {
        buffStateBar.fillAmount = curTime / duration;

        //몇초 남았을 때 깜박거리게 세팅하기.
        if (curTime / duration <= 0.1f)
        {

        }
    }

    private void OnDestroy()
    {
        //버프 매니저에게 버프가 종료 되었음을 알림...
    }
}
