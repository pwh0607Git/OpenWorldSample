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
        // IStateEffect effect = EffectFactory.CreateEffect(EffectType.Damage, 10);
        // uiPresenter.ApplyEffect(effect);
    }
}