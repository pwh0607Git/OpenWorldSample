using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateView : MonoBehaviour
{
    [SerializeField] GameObject HP_Bar;
    [SerializeField] GameObject MP_Bar;
    private Image HP_Image;
    private Image MP_Image;

    void Awake()
    {
        HP_Image = HP_Bar.GetComponentInChildren<Image>();
        MP_Image = MP_Bar.GetComponentInChildren<Image>();
    }

    public void UpdateView(PlayerState p_state){
        Debug.Log($"{p_state.currentHp} / {p_state.state.hp}");
        HP_Image.fillAmount = (float)p_state.currentHp / p_state.state.hp;
        MP_Image.fillAmount = (float)p_state.currentMp / p_state.state.mp;
    }
}