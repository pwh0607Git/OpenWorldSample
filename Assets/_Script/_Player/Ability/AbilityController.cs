using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using System.Linq;

public class AbilityController : MonoBehaviour
{
    [Space(20), ReadOnly] public AbilityFlag flags = AbilityFlag.None;
    public List<Ability> abilities = new();
    
    private readonly Dictionary<AbilityFlag, Ability> originActives = new();
    private readonly Dictionary<AbilityFlag, Ability> actives = new();
    
    private void Update()
    {
        foreach( var a in actives.ToList())
            a.Value.Update();
    }

    private void FixedUpdate()
    {
        foreach( var a in actives.ToList())
            a.Value.FixedUpdate();
    }

    public void Add(AbilityData abilityData, bool immediate = false){
        if (!actives.ContainsKey(abilityData.flag))
        {
            flags.Add(abilityData.flag); 

            var ability = abilityData.CreateAbility(GetComponent<PlayerController1>());
            abilities.Add(ability);
            originActives[abilityData.flag] = ability;
            actives[abilityData.flag] = ability;
        }
    }

    public void Remove(AbilityFlag flag){
        if (actives.ContainsKey(flag))
        {
            flags.Remove(flag, null);
            abilities.Remove(actives[flag]);
            actives.Remove(flag);
        }
    }

    public void Activate(AbilityFlag flag, bool forceDeactivate = false){
        if(forceDeactivate) DeactivateAll(flag);
        
        if(!actives.ContainsKey(flag)) return;

        actives[flag].Activate();
    }

    public void Deactivate(AbilityFlag flag){
        if(!actives.ContainsKey(flag)) return;

        actives[flag].Deactivate();
    }

    public void DeactivateAll(AbilityFlag flag)
    {
        foreach( var a in actives ){
            if(a.Key.Equals(flag)) continue;
            a.Value.Deactivate();
        }
        actives.Clear();
    }

    public void RestoreAbilities(){
        actives.Clear();
        foreach (var kvp in originActives)
        {
            actives[kvp.Key] = kvp.Value;
        }
    }

    public void UpdatePlayerState(PlayerState p_state){
        // 나중에...
    }
}
