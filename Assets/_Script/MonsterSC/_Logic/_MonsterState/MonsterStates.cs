using UnityEngine;

namespace MonsterStates{
    public class MonsterStateIdle : IMonsterState
    {
        // 싱글톤 세팅.
        private static MonsterStateIdle instance;
        public static MonsterStateIdle Instance{
            get{
                if(instance == null)
                    instance = new MonsterStateIdle();
                return instance;
            }
        }

        public static MonsterStateIdle GetInstance(){
            return Instance;
        }

        float waitTimer;
        [SerializeField] Vector3 nextDestination;
        public void EnterState(MonsterControllerFromState monster){
            monster.animator.SetBool("Walk",true);
            waitTimer = 2.0f;
            monster.SetNextDestination();
        }
        public void UpdateState(MonsterControllerFromState monster){
            if(!monster.IsArrivingDestination(monster.transform.position, monster.nextDestination))
            {
                waitTimer -= Time.deltaTime;

                if(waitTimer <=0){
                    monster.SetNextDestination();
                    monster.animator.SetBool("Walk",true);
                }
            }else{
                monster.MoveToward(monster.nextDestination);
            }
        }
        public void ExitState(MonsterControllerFromState monster){

        }
    }
}
