using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;

public class PlayerStates : MonoBehaviour
{
    //ÀÌÈÄ¿¡ ½Ì±ÛÅæÀ¸·Î ¹Ù²ÜÁö °í¹Î...
    public GameObject HP_Bar;
    public GameObject MP_Bar;

    private Image HP_Image;
    private Image MP_Image;

    private State myState;

    //TEST ¿ë ÄÚµå
    int testDamage = 10;
    int testHeal = 20;

    private void Start()
    {
        myState = PlayerController.player.myState;
        HP_Image = HP_Bar.GetComponent<Image>();
        MP_Image = MP_Bar.GetComponent<Image>();
        myState.OnStateChanged += UpdateStateUI;
    }

    public void UpdateStateUI()
    {
        HP_Image.fillAmount = (float)myState.curHP / myState.maxHP;
        MP_Image.fillAmount = (float)myState.curMP / myState.maxMP;
    }

    //TEST Code;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) TakeDamage();

        if (Input.GetKeyDown(KeyCode.F2)) TakeHeal();
    }

    public void TakeDamage()
    {
        myState.curHP -= testDamage;
        UpdateStateUI();
    }

    public void TakeHeal()
    {
        myState.curHP += testHeal;
        UpdateStateUI();
    }
}