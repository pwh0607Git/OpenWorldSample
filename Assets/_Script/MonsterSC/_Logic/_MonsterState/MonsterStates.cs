using UnityEngine;

namespace MonsterStates{
    public class MonsterStateIdle : IMonsterState
    {        
        float waitTimer= 0.0f;
        float waitingTime = 2.0f;
        bool isWaiting = false;

        [SerializeField] Vector3 nextDestination;

        public void EnterState(MonsterControllerFromState monster){
            monster.SetNextDestination();
        }

        public void UpdateState(MonsterControllerFromState monster){
            if(monster.GetAttackTarget() != null){
                monster.TransitionToState(new MonsterStateChase());
                return;
            }

            if(monster.IsArrivingDestination(monster.transform.position, nextDestination))
            {
                if(!isWaiting){
                    monster.animator.SetBool("Walk",false);
                    waitTimer = waitingTime;
                    isWaiting = true;
                }
            }else{
                monster.MoveToward(nextDestination);
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
        public void EnterState(MonsterControllerFromState monster){
            
        }

        public void UpdateState(MonsterControllerFromState monster){
            if(monster.GetAttackTarget() == null){
                monster.TransitionToState(new MonsterStateIdle());
            }

            float distanceToTarget = Vector3.Distance(monster.GetAttackTarget().position, monster.GetAttackTarget().position);

            if(distanceToTarget <= monster.monsterData.attackableRadius){
                monster.TransitionToState(new MonsterStateAttack());
            }else{
                monster.ChasePlayer();
            }
        }

        public void ExitState(MonsterControllerFromState monster){
            
        }
    }
    
    public class MonsterStateAttack : IMonsterState{
        private float attackCoolTime = 1.5f;
        private float attackTimer;

        public void EnterState(MonsterControllerFromState monster){
            monster.animator.SetTrigger("Attack");
            monster.animator.SetBool("Walk", false);
            attackTimer = attackCoolTime;
        }
        public void UpdateState(MonsterControllerFromState monster){
            if(monster.GetAttackTarget() == null){
                monster.TransitionToState(new MonsterStateIdle());
                return;
            }

            Vector3 directionToTarget = monster.GetAttackTarget().position - monster.transform.position;
            float distanceToTarget = directionToTarget.magnitude;

            if (distanceToTarget > monster.monsterData.attackableRadius)
            {
                monster.TransitionToState(new MonsterStateChase());
            }
        }

        public void ExitState(MonsterControllerFromState monster){
            
        }
    }
        
    public class MonsterStateDown : IMonsterState{
        public void EnterState(MonsterControllerFromState monster){
        
        }

        public void UpdateState(MonsterControllerFromState monster){
        
        }

        public void ExitState(MonsterControllerFromState monster){
        
        }
    }
}