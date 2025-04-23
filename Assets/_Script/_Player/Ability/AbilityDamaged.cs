using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class AbilityDamaged : Ability<AbilityDamagedData>
{
    public override AbilityFlag Flag => AbilityFlag.Damaged;
    float abilityDuration;
    private bool isPerforming = false;
    public override void Activate() {
        player.abilityController.Activate(Flag, true);
        isPerforming = true;
        TakeDamage();
    }

    public override void Deactivate() { 
        isPerforming = false;
    }

    public AbilityDamaged(AbilityDamagedData data, PlayerController1 player) : base(data,player){
        float animationSpeed = 1f;
        abilityDuration = data.duration;
    }

    public void TakeDamage(){

        CoolTimeAsync().Forget();
        DG.Tweening.Sequence damageSeq = DOTween.Sequence(player.gameObject);
        damageSeq.AppendCallback(() => PlayAnimation("TakeDamage", 0.02f, 0, 0f));
        damageSeq.OnComplete(()=>Deactivate());
    }

    async UniTaskVoid CoolTimeAsync(){
        try{
            isPerforming = true;
            await UniTask.WaitForSeconds(abilityDuration);
            isPerforming = false;
        }catch(System.Exception e){
            Debug.LogException(e);
        }
    }
}
