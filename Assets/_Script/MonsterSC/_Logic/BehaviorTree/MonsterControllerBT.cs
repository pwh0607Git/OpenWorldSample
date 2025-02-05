using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControllerBT : MonoBehaviour
{
    #region ReFactoring
    [SerializeField] private BTNode rootNode;
    private MonsterStateUIController monsterUI;
    
    [SerializeField] private MonsterBlackBoard blackBoard = null;
    [SerializeField] private MonsterAttack attack;
    [SerializeField] private MonsterDetection detection;
    [SerializeField] private MonsterHealth health;
    [SerializeField] private MonsterWandor wandor;
    #endregion

    private float rotationSpeed = 20.0f;
    private float speedWeight = 3.0f;

    public MonsterHealth GetMonsterlogic(){
        return health;
    }

    public void SetMonsterUI(MonsterStateUIController monsterUI)
    {
        this.monsterUI = monsterUI;
    }
    
    // //animation Event => 지우지 말기.
    // public void PerformAttack(){
    //     Vector3 attackOffset = transform.localPosition + Vector3.up/2 + transform.forward;
    //     Collider[] hitTargets = Physics.OverlapSphere(attackOffset, blackBoard.monsterData.attackDamageRadius);

    //     if(hitTargets.Length == 0){
    //         transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation, Time.deltaTime * 5.0f);
    //     }else{
    //         foreach(var target in  hitTargets)
    //         {
    //             if (target.CompareTag("Player"))
    //             {
    //                 target.GetComponentInChildren<PlayerController>().PlayerTakeDamage(blackBoard.monsterData.attackPower);
    //             }
    //         }
    //     }
    // }

    private bool isDown;
    
    public bool IsDownMonster(){
        return isDown && blackBoard.currentHP <= 0;
    }

    public void DownMonster(){
        Debug.Log("monster Down...!");
        blackBoard.animator.SetTrigger("Down");
        Invoke("DestroyMonster", 1f);
    }

    private void DestroyMonster(){
        //Loot Handler Code Part
        Destroy(gameObject);
    }

    private void Start()
    {
        TestInitBB();
        StartCoroutine(SetMonsterLogic());
        StartCoroutine(InitBTNode());
    }

    private void Update()
    {
        rootNode.Evaluate();
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
    }
    
    public void InitMonsterBlackBoard(MonsterBlackBoard blackBoard){
        this.blackBoard = blackBoard;
    }

    IEnumerator InitBTNode(){
        while(blackBoard == null){
            yield return null;
        }
        Debug.Log("BT Node Init Complete");
        rootNode = new Selector(new List<BTNode>
        {
            new Sequence(new List<BTNode>{
                new ConditionNode(IsDownMonster),
                new ActionNode(DownMonster)
            }),
            new Sequence(new List<BTNode>
            {
                new ConditionNode(()=>blackBoard.isMonsterDamaged),      
                new ActionNode(health.HandleDamageAnim),
                new WaitNode(1f),
                new LookAtTargetNode(transform, blackBoard.player, blackBoard.animator, rotationSpeed),  
                new ActionNode(detection.ChaseTarget),
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
                new ActionNode(detection.ChaseTarget)
            }),
            new ActionNode(wandor.Wandor)    
        });
    }

    public void TakeDamage(int damage){
        health.TakeDamage(damage);
    }
    [SerializeField] MonsterData monsterData;           // destroy

    void TestInitBB(){
        blackBoard = gameObject.AddComponent<MonsterBlackBoard>();
        blackBoard.monsterData = monsterData;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, monsterData.detectionRadius);

        Vector3 forward = transform.forward * monsterData.detectionRadius;
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
        Gizmos.DrawWireSphere(attackOffset, monsterData.attackDamageRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, monsterData.attackableRadius);
    }
}