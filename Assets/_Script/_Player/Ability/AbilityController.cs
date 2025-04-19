using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using System.Linq;

public class AbilityController : MonoBehaviour
{
    [Space(20), ReadOnly] public AbilityFlag flags = AbilityFlag.None;
    public List<Ability> abilities = new();
    public AbilityFlag abilityFlags = AbilityFlag.None; 
    private readonly Dictionary<AbilityFlag, Ability> actives = new Dictionary<AbilityFlag, Ability>();

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

    public void Add(AbilityFlag flag, Ability ability, bool immediate = false){
        if (!actives.ContainsKey(flag))
        {
            flags |= flag; 
            abilities.Add(ability);
            abilityFlags.Add(flag);
            actives[flag] = ability;
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
        if(forceDeactivate) DeactivateAll();
        
        if(!actives.ContainsKey(flag)) return;
        actives[flag].Activate();
        abilityFlags.Add(flag);
    }

    public void Deactivate(AbilityFlag flag){
        if(!actives.ContainsKey(flag)) return;

        actives[flag].Deactivate();
        abilityFlags.Remove(flag);
    }

    public void DeactivateAll()
    {
        foreach( var a in actives )
            a.Value.Deactivate();
        actives.Clear();
        abilityFlags = AbilityFlag.None;
    }

    public bool IsActive(AbilityFlag flag)
    {
        return actives.ContainsKey(flag);
    }
    
    public void UpdatePlayerState(PlayerState p_state){
        // 나중에...
    }
}
