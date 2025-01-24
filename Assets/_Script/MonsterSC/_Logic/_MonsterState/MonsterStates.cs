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
        

        float waitTimer= 0.0f;
        float waitingTime = 2.0f;
        bool isWaiting = false;
        [SerializeField] Vector3 nextDestination;
        public void EnterState(MonsterControllerFromState monster){
            monster.SetNextDestination();
        }
        public void UpdateState(MonsterControllerFromState monster){
            if(monster.attackTarget != null){
                monster.TransitionToState(MonsterStateChase.GetInstance());
                return;
            }

            if(monster.IsArrivingDestination(monster.transform.position, monster.nextDestination))
            {
                if(!isWaiting){
                    monster.animator.SetBool("Walk",false);
                    waitTimer = waitingTime;
                    isWaiting = true;
                }
            }else{
                monster.MoveToward(monster.nextDestination);
            }

            if (isWaiting)
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    isWaiting = false;
                    monster.SetNextDestination();
                }
            }
        }
        public void ExitState(MonsterControllerFromState monster){
            
        }
    }

    public class MonsterStateChase : IMonsterState{
        private static MonsterStateChase instance;
        public static MonsterStateChase Instance{
            get{
                if(instance == null)
                    instance = new MonsterStateChase();
                return instance;
            }
        }

        public static MonsterStateChase GetInstance(){
            return Instance;
        }

        public void EnterState(MonsterControllerFromState monster){
            
        }

        public void UpdateState(MonsterControllerFromState monster){
            if(monster.attackTarget == null){
                monster.TransitionToState(MonsterStateIdle.GetInstance());
            }

            float distanceToTarget = Vector3.Distance(monster.attackTarget.position, monster.transform.position);

            if(distanceToTarget <= monster.monsterData.attackableRadius){
                monster.TransitionToState(MonsterStateBattle.GetInstance());
            }else{
                monster.ChasePlayer();
            }
        }

        public void ExitState(MonsterControllerFromState monster){
            
        }
    }
    
    public class MonsterStateBattle : IMonsterState{
        private static MonsterStateBattle instance;
        public static MonsterStateBattle Instance{
            get{
                if(instance == null)
                    instance = new MonsterStateBattle();
                return instance;
            }
        }

        public static MonsterStateBattle GetInstance(){
            return Instance;
        }
        private float attackCoolTime = 1.5f;
        private float attackTimer;

        public void EnterState(MonsterControllerFromState monster){
            monster.animator.SetTrigger("Attack");
            monster.animator.SetBool("Walk", false);
            attackTimer = attackCoolTime;
        }
        public void UpdateState(MonsterControllerFromState monster){
            if(monster.attackTarget == null){
                monster.TransitionToState(MonsterStateIdle.GetInstance());
                return;
            }

            Vector3 directionToTarget = monster.attackTarget.position - monster.transform.position;
            float distanceToTarget = directionToTarget.magnitude;

            if (distanceToTarget > monster.monsterData.attackableRadius)
            {
                monster.TransitionToState(MonsterStateChase.GetInstance());
            }
        }

        public void ExitState(MonsterControllerFromState monster){
            
        }
    }
        
    public class MonsterStateDown : IMonsterState{
        private static MonsterStateBattle instance;
        public static MonsterStateBattle Instance{
            get{
                if(instance == null)
                    instance = new MonsterStateBattle();
                return instance;
            }
        }
        public void EnterState(MonsterControllerFromState monster){
        
        }

        public void UpdateState(MonsterControllerFromState monster){
        
        }

        public void ExitState(MonsterControllerFromState monster){
        
        }
    }
}