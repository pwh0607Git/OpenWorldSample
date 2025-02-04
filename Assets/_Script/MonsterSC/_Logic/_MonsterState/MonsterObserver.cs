using UnityEngine;

public abstract class MonsterObserver 
{
    public abstract void OnNotify(int curHP);
}

public class UIObserver : MonsterObserver 
{
    public override void OnNotify(int curHP)
    {
        Debug.Log("몬스터 피격! HP바 갱신하기...");
        
    }
}