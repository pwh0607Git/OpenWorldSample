using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStateUIController : MonoBehaviour
{
    MonsterData monsterData;

    public TextMeshProUGUI monsterName;            //몬스터 이름.
    public TextMeshProUGUI curHPTxt;               //현재 체력을 txt형식으로 출력

    public Image HP_Bar;

    public GameObject damageText;          //몬스터가 데미지를 입었을 때, 출력되는 데미지 이펙트.

    public void InitMonsterUI(MonsterData monsterData)
    {
        Debug.Log("몬스터 UI 초기화");
        GetComponent<RectTransform>().localPosition = Vector2.zero;
        this.monsterData = monsterData;
        monsterName.text = monsterData.monsterName;
        UpdateMonsterUI(monsterData.HP);
    }

    public void UpdateMonsterUI(int curHP)
    {
        curHPTxt.text = curHP.ToString();
        HP_Bar.fillAmount = (float)curHP / monsterData.HP;
    }

    public void ShowDamage(int damage)
    {
        /*
        GameObject damageIcon = Instantiate(damageText);
        damageIcon.GetComponent<MonsterDamage>().SetDamage(damage);
        */
    }
}
