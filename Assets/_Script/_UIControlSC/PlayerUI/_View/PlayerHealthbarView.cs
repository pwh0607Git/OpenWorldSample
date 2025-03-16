using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbarView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] Image hp_Image;
    [SerializeField] Image mp_Image;

    public void UpdateView(PlayerState p_state){
        Debug.Log($"{p_state.currentHp} / {p_state.state.hp}");
        hp_Image.fillAmount = (float)p_state.currentHp / p_state.state.hp;
        mp_Image.fillAmount = (float)p_state.currentMp / p_state.state.mp;
        level.text = p_state.level.ToString();
    }
}
