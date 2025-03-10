using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class PlayerStateTester : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] int damage = 10;

    [Header("Reference")]
    public PlayerUIPresenter uiPresenter;
    [Button("DamageTester"), HideField] public bool btn1;

    public void DamageTester(){
        uiPresenter.TakeDamage(damage);
    }
}