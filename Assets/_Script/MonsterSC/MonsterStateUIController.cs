using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStateUIController : MonoBehaviour
{
    MonsterData monsterData;

    public TextMeshProUGUI monsterName;            //몬스터 이름.
    public TextMeshProUGUI curHPText;               //현재 체력을 txt형식으로 출력

    public Image HP_Bar;

    public GameObject damageTextPrefab;          //몬스터가 데미지를 입었을 때, 출력되는 데미지 이펙트.
    public Transform damageTextTransform;

    public void InitMonsterUI(MonsterData monsterData)
    {
        this.monsterData = monsterData;
        monsterName.text = monsterData.monsterName;
        
        GetComponent<RectTransform>().localPosition = Vector2.zero;
        UpdateMonsterUI(monsterData.HP);
    }

    public void UpdateMonsterUI(int curHP)
    {
        curHPText.text = curHP.ToString();
        HP_Bar.fillAmount = (float)curHP / monsterData.HP;
    }

    public void ShowDamage(int damage)
    {
        GameObject damageText = Instantiate(damageTextPrefab, damageTextTransform);
        damageText.GetComponent<MonsterDamage>().SetDamage(damage);
        damageText.SetActive(true);
    }
}
