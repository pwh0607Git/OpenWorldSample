using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControllerBT : MonoBehaviour
{
    #region ReFactoring
    [SerializeField] private BTNode rootNode;
    private MonsterStateUIController monsterUI;
    
    [SerializeField] private MonsterBlackBoard blackBoard = null;
    [SerializeField] private MonsterHealth health;
    [SerializeField] private MonsterWandor wandor;
    [SerializeField] private MonsterDetection detection;
    [SerializeField] private MonsterChase chase;
    [SerializeField] private MonsterAttack attack;
    [SerializeField] private MonsterDown down;
    #endregion

    public MonsterHealth GetMonsterlogic(){
        return health;
    }

    public void SetMonsterUI(MonsterStateUIController monsterUI)
    {
        this.monsterUI = monsterUI;
    }

    private void Start()
    {
        FixOriginalPosition();
        StartCoroutine(SetMonsterLogic());
        StartCoroutine(InitBTNode());
    }

    private void Update()
    {
        rootNode.Evaluate();
    }
    void FixOriginalPosition(){
        var spawnController = GetComponentInChildren<ObjectSpawnInitController>();
        spawnController.SetOntheFloor();
        blackBoard.originalPosition = spawnController.originalPosition;
        transform.position = blackBoard.originalPosition;            
    }
    IEnumerator SetMonsterLogic(){
        while(blackBoard == null){
            yield return null;
        }
        Debug.Log("MonsterLogic Init Complete");
        wandor = gameObject.AddComponent<MonsterWandor>();
        attack = gameObject.AddComponent<MonsterAttack>();
        detection = gameObject.AddComponent<MonsterDetection>();
        health = gameObject.AddComponent<MonsterHealth>();
        chase = gameObject.AddComponent<MonsterChase>();
        down = gameObject.AddComponent<MonsterDown>();
    }
    
    public void InitMonsterBlackBoard(MonsterBlackBoard blackBoard){
        this.blackBoard = blackBoard;
    }

    float chaseStartTime = 0f;
    IEnumerator InitBTNode(){
        while(blackBoard == null){
            yield return null;
        }
        
        rootNode = new Selector(new List<BTNode>
        {
            new ConditionNode(() => blackBoard.isDown),
            new Sequence(new List<BTNode>{
                new ConditionNode(down.IsDownMonster),
                new ActionNode(down.DownMonster),
            }),
            new Sequence(new List<BTNode>
            {
                new ConditionNode(()=>blackBoard.isMonsterDamaged),      
                new ActionNode(health.HandleDamageAnim),
                new WaitNode(1f),
                new LookAtTargetNode(transform, blackBoard.player, blackBoard.animator),  
                new ConditionNode(chase.IsTargetInChasingRange),
                new ActionNode(chase.ChaseTarget),
            }),
            new Sequence(new List<BTNode>
            {
                new ConditionNode(attack.CheckTargetInAttackRange),
                new ActionNode(attack.AttackTarget)
            }),
            new Sequence(new List<BTNode>
            {
                new ConditionNode(detection.IsTargetInDetectionRange),
                new ConditionNode(detection.IsExistingObject),
                new ActionNode(() => 
                { 
                    blackBoard.isDetecting = true; // 플레이어 감지 시작
                }),
                new ActionNode(chase.ChaseTarget),   // 계속 추적
            }),
            new ActionNode(wandor.Wandor), // 배회 지속
        });
    }

    public void TakeDamage(int damage){
        health.TakeDamage(damage);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, blackBoard.monsterData.detectionRadius);

        Vector3 forward = transform.forward * blackBoard.monsterData.detectionRadius;
        Quaternion leftRayRotation = Quaternion.Euler(0, -80f / 2, 0);
        Quaternion rightRayRotation = Quaternion.Euler(0, 80 / 2, 0);

        Vector3 leftRay = leftRayRotation * forward;
        Vector3 rightRay = rightRayRotation * forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, leftRay);
        Gizmos.DrawRay(transform.position, rightRay);

        //Attack Gizmo
        Gizmos.color = Color.red;
        Vector3 attackOffset = transform.localPosition + Vector3.up / 2 + transform.forward;
        Gizmos.DrawWireSphere(attackOffset, blackBoard.monsterData.attackDamageRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, blackBoard.monsterData.attackableRadius);

        //Chasing Gizmo
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, blackBoard.monsterData.chasingRadius);
    }
}