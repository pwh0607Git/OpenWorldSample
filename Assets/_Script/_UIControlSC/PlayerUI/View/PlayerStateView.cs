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
    IEnumerator Start(){
        yield return new WaitUntil(() => p_state != null);
        
        HP_Image = HP_Bar.GetComponent<Image>();
        MP_Image = MP_Bar.GetComponent<Image>();
        p_state.OnStateChanged += UpdateStateUI;
    }

    public void UpdateStateUI()
    {
        HP_Image.fillAmount = (float)p_state.curHP / p_state.maxHP;
        MP_Image.fillAmount = (float)p_state.curMP / p_state.maxMP;
    }
}
