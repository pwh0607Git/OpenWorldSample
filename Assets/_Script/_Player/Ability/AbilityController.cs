using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using System.Linq;

public class AbilityController : MonoBehaviour
{
    [Space(20), ReadOnly] public AbilityFlag flags = AbilityFlag.None;
    public List<Ability> abilities = new();
    private readonly Dictionary<AbilityFlag, Ability> actives = new Dictionary<AbilityFlag, Ability>();
    
    private void FixedUpdate()
    {
        foreach( var a in actives.ToList()){
            a.Value.FixedUpdate();
        }
    }

    public void Add(AbilityFlag flag, Ability ability, bool immediate = false){
        if (!actives.ContainsKey(flag))
        {
            abilities.Add(ability);
            actives[flag] = ability;
        }
    }

    public void Remove(AbilityFlag flag, Ability ability){
        if (actives.ContainsKey(flag))
        {
            abilities.Remove(ability);
            actives.Remove(flag);
        }
    }

    public void Activate(AbilityFlag flag){
        if(!actives.ContainsKey(flag)) return;
        actives[flag].Activate();
    }

    public void Deactivate(AbilityFlag flag){
        if(!actives.ContainsKey(flag)) return;
        actives[flag].Deactivate();
    }

    public void UpdatePlayerState(PlayerState p_state){
        // 나중에...
    }
}
