using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateView : MonoBehaviour
{
    
    [SerializeField] GameObject HP_Bar;
    [SerializeField] GameObject MP_Bar;
    private Image HP_Image;
    private Image MP_Image;
    [SerializeField] PlayerState p_state;

    void Awake()
    {
        HP_Image = HP_Bar.GetComponentInChildren<Image>();
        MP_Image = MP_Bar.GetComponentInChildren<Image>();
    }

    public void UpdateView(PlayerState state){
        p_state = state;
        HP_Image.fillAmount = (float)state.currentHp / state.maxHp;
        MP_Image.fillAmount = (float)state.currentHp / state.maxHp;
    }
}