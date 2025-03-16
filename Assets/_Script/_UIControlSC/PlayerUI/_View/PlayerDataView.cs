using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using CustomInspector;
using TMPro;

public class PlayerDataView : MonoBehaviour
{
    #region PlayerData
    [SerializeField] TextMeshProUGUI tmp_MaxHp;
    [SerializeField] TextMeshProUGUI tmp_MaxMp;
    [SerializeField] TextMeshProUGUI tmp_Attack;
    [SerializeField] TextMeshProUGUI tmp_Defend;
    
    [SerializeField] TextMeshProUGUI tmp_Speed;
    
    public void UpdatePlayerDataView(PlayerState p_state){
        tmp_MaxHp.text = p_state.state.hp.ToString();
        tmp_MaxMp.text = p_state.state.mp.ToString();
        tmp_Attack.text = p_state.state.attack.ToString();
        tmp_Defend.text = p_state.state.defend.ToString();
        tmp_Speed.text = p_state.state.speed.ToString();
    }
    
    #endregion
}