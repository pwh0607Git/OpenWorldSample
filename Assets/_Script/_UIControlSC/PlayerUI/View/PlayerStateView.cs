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
    IEnumerator Start(){
        yield return new WaitUntil(() => p_state != null);
        p_state.OnStateChanged += UpdateStateUI;
    }

    public void UpdateStateUI()
    {
        if (p_state == null) return;
        HP_Image.fillAmount = (float)p_state.curHP / p_state.maxHP;
        MP_Image.fillAmount = (float)p_state.curMP / p_state.maxMP;
    }

    public void UpdateView(PlayerState state){
        if (state == null) return;
        p_state = state;
        UpdateStateUI();
    }
}
