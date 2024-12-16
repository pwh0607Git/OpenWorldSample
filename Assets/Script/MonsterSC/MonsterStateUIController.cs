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

    private int curHP;
    public Image HP_Bar;

    public GameObject damageText;          //몬스터가 데미지를 입었을 때, 출력되는 데미지 이펙트.

    public void InitMonsterUI(MonsterData monsterData)
    {
        GetComponent<RectTransform>().localPosition = Vector2.zero;
        this.monsterData = monsterData;
        monsterName.text = monsterData.monsterName;
        curHP = monsterData.HP;
        UpdateMonsterUI();
    }

    public void TakeDamage(int damage)
    {
        curHP -= damage;
        /*
        GameObject damageIcon = Instantiate(damageText);
        damageIcon.GetComponent<MonsterDamage>().SetDamage(damage);
        */
        Debug.Log($"현재 받은 데미지 : {damage}, {curHP}");

        UpdateMonsterUI();
    }

    public void UpdateMonsterUI()
    {
        curHPTxt.text = curHP.ToString();
        HP_Bar.fillAmount = (float)curHP / monsterData.HP;
    }
}
