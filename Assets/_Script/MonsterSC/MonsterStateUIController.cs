using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStateUIController : MonoBehaviour
{
    MonsterData monsterData;

    public TextMeshProUGUI monsterName;            
    public TextMeshProUGUI curHPText;              

    public Image HP_Bar;

    public GameObject damageTextPrefab;          
    public Transform damageTextTransform;

    void Update(){
        gameObject.GetComponent<RectTransform>().LookAt(Camera.main.transform);
    }

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
