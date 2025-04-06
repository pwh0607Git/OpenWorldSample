using UnityEngine.Events;

public static class AbilityExtension
{
    public static void Set(ref this AbilityFlag abilities, AbilityFlag a)
    {
        abilities = a;
    }

    // a Ability 보유 여부
    public static bool Has(ref this AbilityFlag abilities, AbilityFlag a)
    {
        return (abilities & a) == a;
    }

    public static bool HasAny(ref this AbilityFlag abilities, AbilityFlag a)
    {
        return (abilities & a) != 0;
    }

    // a Ability 추가
    public static void Add(ref this AbilityFlag abilities, AbilityFlag a, UnityAction onComplete = null){
        abilities |= a;
        onComplete?.Invoke();
    }

    // a Ability 제거
    public static void Remove(ref this AbilityFlag abilities, AbilityFlag a, UnityAction onComplete = null){
        abilities &= ~a;
        onComplete?.Invoke();
    }

    // a Ability 사용 -> 액션 발동.
    public static void Use(ref this AbilityFlag abilities, AbilityFlag a, UnityAction onComplete){
        if(abilities.Has(a)){
            onComplete?.Invoke();
        } 
    }
}