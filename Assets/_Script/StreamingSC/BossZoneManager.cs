using UnityEngine;

public class BossZoneManager : MonoBehaviour
{
    // 각각의 씬에 존재하는 보스전 관리자.
    // 캐릭터가 보스전에 입장식 BossManager에게 호출.

    [SerializeField] bool isPerformingStage;          //보스전에 입장해있는 캐릭터
    [SerializeField] float BossZoneArea = 100f;
    [SerializeField] string bossId;
    [SerializeField] BossInform stageBoss;
    void OnEnable(){
        isPerformingStage = false;
    }

    void Update(){
        if(!isPerformingStage){
            float distance = Vector3.Distance(PlayerController.player.transform.position, transform.position);
            if(distance <= BossZoneArea){
                EnterPlayer();
            }
        }
    }

    void EnterPlayer(){
        isPerformingStage = true;
        stageBoss = BossManager.bossManager.StartBossStage(bossId);       //Character의 보스 관련 데이터 시작
    }

    void OutPlayer(){

    }
}
