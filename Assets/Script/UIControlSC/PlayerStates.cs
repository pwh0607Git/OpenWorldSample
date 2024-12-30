using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;

public class PlayerStates : MonoBehaviour
{
    //이후에 싱글톤으로 바꿀지 고민...
    public GameObject HP_Bar;
    public GameObject MP_Bar;

    private Image HP_Image;
    private Image MP_Image;

    private State myState;

    public Action<GameObject> OnBuffEnd; // 버프 종료 시 실행할 콜백

    private void Start()
    {
        myState = PlayerController.player.myState;
        HP_Image = HP_Bar.GetComponent<Image>();
        MP_Image = MP_Bar.GetComponent<Image>();
        //myState.OnStateChanged += UpdateStateUI;
    }

    public void UpdateStateUI()
    {
        HP_Image.fillAmount = (float)myState.curHP / myState.maxHP;
        MP_Image.fillAmount = (float)myState.curMP / myState.maxMP;
    }
}