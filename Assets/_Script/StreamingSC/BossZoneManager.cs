using System;
using UnityEngine;

public class BossZoneManager : MonoBehaviour
{
    // 각각의 씬에 존재하는 보스전 관리자.
    // 캐릭터가 보스전에 입장식 BossManager에게 호출.
    // BossManager는 데이터 관리용으로만 사용

    [SerializeField] bool isSpawningBoss;           // 보스가 소환되어있는 상태인지.
    [SerializeField] bool isPerformingStage;        // 보스전이 진행중인지...
    [SerializeField] float bossZoneArea = 100f;
    [SerializeField] string bossId;
    [SerializeField] BossInform stageBossData;

    public Action action;
    void Start(){
        stageBossData = BossManager.bossManager.StartBossStage(bossId);       //Character의 보스 관련 데이터 시작
        isSpawningBoss = false;
        SetBoss();
    }

    void SetBoss(){
        // if(Time.time - stageBossData.deathTime < 30.0f || isSpawningBoss) return; 
        GameObject boss = Instantiate(stageBossData.bossModel);
        isSpawningBoss = true;
    }
    private void OnEnable()
    {
        // 보스 사망 이벤트 구독
        Boss.OnBossDown += HandleBossDeath;
    }

    private void OnDisable()
    {
        // 구독 해제 (메모리 누수 방지)
        Boss.OnBossDown -= HandleBossDeath;
    }

    private void HandleBossDeath()
    {
        Debug.Log("BossZoneManage : 보스가 죽었습니다. 다음 단계 진행!");
    }

    void Update(){
        float distance = Vector3.Distance(PlayerController.player.transform.position, transform.position);
        
        if(!isPerformingStage){
            if(distance <= bossZoneArea){
                EnterPlayer();
            }
        }else{
            if(distance > bossZoneArea){
                OutPlayer();
            }
        }

        // SetBoss();
    }

    void EnterPlayer(){
        Debug.Log("Player가 보스 위치에 입장!");
        isPerformingStage = true;
    }

    void OutPlayer(){
        Debug.Log("Player가 보스 위치에 퇴장!");
        isPerformingStage = false;
    }
    void OnDeathBoss(){
        // 보스가 처치 되었을 경우...
        stageBossData.deathTime = Time.time;
        BossManager.bossManager.UpdateBossInform(bossId, stageBossData);
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, bossZoneArea);
    }

    public void CheckBossState(){

    }
}
