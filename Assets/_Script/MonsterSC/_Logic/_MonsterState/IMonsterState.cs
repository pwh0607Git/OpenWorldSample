public interface IMonsterState
{
    public void EnterState(MonsterControllerFromState monster);
    public void UpdateState(MonsterControllerFromState monster);
    public void ExitState(MonsterControllerFromState monster);   
}