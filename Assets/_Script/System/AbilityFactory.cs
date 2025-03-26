public static class AbilityFactory
{

    public static Ability CreateAbility(AbilityFlag flag, PlayerState p_state, PlayerController1 player, AttackArea area = null){
        return flag switch{
            AbilityFlag.Attack => new AbilityAttack(p_state, player, area),
            AbilityFlag.Damaged => new AbilityDamaged(p_state, player),
            // AbilityFlag.Dodge => new AbilityDod
            //
            _=> null
        };
    }
}
