using UnityEngine;

public static class AnimationUtility
{
    public static float GetAnimationClipLength(this  Animator animator, string clipName){
        if(animator == null) return 0.3f;

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips){
            if (clip.name == clipName) return clip.length; 
        }
        return 0.3f;
    } 
}