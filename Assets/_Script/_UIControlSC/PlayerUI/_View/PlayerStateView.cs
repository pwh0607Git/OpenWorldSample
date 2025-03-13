using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateView : MonoBehaviour
{
    [SerializeField] GameObject hp_Bar;
    [SerializeField] GameObject mp_Bar;
    [SerializeField] private TextMeshProUGUI level;
    private Image hp_Image;
    private Image mp_Image;

    void Awake()
    {
        hp_Image = hp_Bar.GetComponentInChildren<Image>();
        mp_Image = mp_Bar.GetComponentInChildren<Image>();
    }

    public void UpdateView(PlayerState p_state){
        Debug.Log($"{p_state.currentHp} / {p_state.state.hp}");
        hp_Image.fillAmount = (float)p_state.currentHp / p_state.state.hp;
        mp_Image.fillAmount = (float)p_state.currentMp / p_state.state.mp;
        level.text = p_state.level.ToString();
    }
}