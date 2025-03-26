using UnityEngine.Events;

public static class AbilityExtension
{
    // a Ability 보유 여부
    public static bool Has(this AbilityFlag abilities, AbilityFlag a)
    {
        return (abilities & a) == a;
    }

    // a Ability 추가
    public static void Add(this AbilityFlag abilities, AbilityFlag a, UnityAction onComplete){
        abilities |= a;
        onComplete?.Invoke();
    }

    // a Ability 제거
    public static void Remove(this AbilityFlag abilities, AbilityFlag a, UnityAction onComplete){
        abilities &= ~a;
        onComplete?.Invoke();
    }

    // a Ability 사용 -> 액션 발동.
    public static void Use(this AbilityFlag abilities, AbilityFlag a, UnityAction onComplete){
        if(abilities.Has(a)){
            onComplete?.Invoke();
        } 
    }
}