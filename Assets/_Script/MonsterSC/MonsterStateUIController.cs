using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStateUIController : MonoBehaviour
{
    private MonsterData monsterData;

    private GameObject rootMonster;
    public TextMeshProUGUI monsterName;

    public Image HP_Bar;

    public GameObject damageTextPrefab;          
    public Transform damageTextTransform;

    void Start(){
        rootMonster = transform.root.gameObject;
        this.monsterData = rootMonster.GetComponent<TEST_MonsterController>().MonsterData;
        
        InitMonsterUI();
        InitMonsterUIPosition();
  }

    void Update(){
        gameObject.GetComponent<RectTransform>().LookAt(Camera.main.transform);
    }

    public void InitMonsterUI()
    {
        monsterName.text = monsterData.monsterName;
        
        GetComponent<RectTransform>().localPosition = Vector2.zero;
        UpdateMonsterUI(monsterData.HP);
    }

    public void InitMonsterUIPosition()
    {
        Collider monsterCollider = rootMonster.GetComponentInChildren<Collider>();

        if (monsterCollider != null)
        {
            float monsterHeight = monsterCollider.bounds.size.y;
            transform.SetParent(monsterCollider.gameObject.transform);
            GetComponent<RectTransform>().anchoredPosition = new Vector3(0, monsterHeight - 0.5f, 0);
        }
        transform.GetComponentInParent<TEST_MonsterController>().SetMonsterUI(this);
    }

    public void UpdateMonsterUI(int curHP)
    {
        HP_Bar.fillAmount = (float)curHP / monsterData.HP;
    }

    public void ShowDamage(int damage)
    {
        GameObject damageText = Instantiate(damageTextPrefab, damageTextTransform);
        damageText.GetComponent<MonsterDamage>().SetDamage(damage);
        damageText.SetActive(true);
    }
}