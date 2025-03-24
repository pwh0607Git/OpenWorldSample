using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBossState : MonoBehaviour
{
    public TextMeshPro bossName;
    public GameObject bossHp;
    private Image boss_Hp_Image;
    [SerializeField] BossZoneManager currentBossZone;
    
    void OEnable()
    {
        if(currentBossZone == null) return;
        currentBossZone.SubscribeToHpChanged(UpdateBossUI);
    }

    void OnDisable()
    {
        if(currentBossZone == null) return;
        currentBossZone.UnsubscribeFromHpChanged(UpdateBossUI); 
        currentBossZone = null;
    }

    public void InitBossData(BossInform bossInform){
        bossName.text = bossInform.bossId;
        boss_Hp_Image.fillAmount = 1;
    }
    void UpdateBossUI(float percent){
        boss_Hp_Image.fillAmount = percent;
    }
}
