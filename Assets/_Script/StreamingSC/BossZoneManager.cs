using System;
using UnityEngine;

public class BossZoneManager : MonoBehaviour
{
    // 각각의 씬에 존재하는 보스전 관리자.
    // 캐릭터가 보스전에 입장식 BossManager에게 호출.
    // BossManager는 데이터 관리용으로만 사용
    [SerializeField] float bossZoneArea = 100f;
    [SerializeField] string bossId;
    [SerializeField] BossInform stageBossData;
    [SerializeField] Boss boss = null;
    public Action action;
    void Start(){
        stageBossData = BossManager.bossManager.StartBossStage(bossId);                     //Character의 보스 관련 데이터 시작
        boss = null;
        SpawnBoss();
    }

    void SpawnBoss(){
        // if(Time.time - stageBossData.deathTime < 30.0f || boss != null) return; 
        boss = Instantiate(stageBossData.bossModel).GetComponent<Boss>();
        if(boss != null){
            boss.SubscribeToHpChanged(NotifyCurrentBossHP);
            boss.SubscribeToBossDown(HandleBossDown);
        }
    }

    private void HandleBossDown()
    {
        boss.UnsubscribeFromBossDown(HandleBossDown);
        stageBossData.deathTime = Time.time;
        BossManager.bossManager.UpdateBossInform(bossId, stageBossData);
    }

    void EnterPlayer(){
        Debug.Log("Player가 보스 위치에 입장!");
        boss.StartBossStage();
    }

    void OutPlayer(){
        Debug.Log("Player가 보스 위치에 퇴장!");
        boss.EndBossStage();
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, bossZoneArea);
    }

    private void NotifyCurrentBossHP(float percent){
        Debug.Log("보스가 데미지를 입었다!! UI를 갱신하자!!");
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            EnterPlayer();
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player")){
            OutPlayer();
        }
    }
}
