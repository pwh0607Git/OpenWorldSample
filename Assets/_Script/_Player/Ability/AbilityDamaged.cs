using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class AbilityDamaged : Ability<PlayerState>
{
    float abilityDuration;
    private bool isPerforming = false;
    public override void Activate() {
        isPerforming = true;
        TakeDamage();
    }

    public override void Deactivate() { 
        isPerforming = false;
    }

    public AbilityDamaged(PlayerState data, PlayerController1 player) : base(data,player){
        float animationSpeed = player.animator.GetFloat("DAMAGEDSPEED");
        abilityDuration = player.animator.GetAnimationClipLength("TakeDamage") / animationSpeed;
    }

    public void TakeDamage(){

        CoolTimeAsync().Forget();
        DG.Tweening.Sequence damageSeq = DOTween.Sequence(player.gameObject);
        damageSeq.AppendCallback(() => PlayAnimation());
        damageSeq.OnComplete(()=>Deactivate());
    }

    private void PlayAnimation(){
        player.animator.CrossFadeInFixedTime("TakeDamage", 0.02f, 0, 0f);
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
