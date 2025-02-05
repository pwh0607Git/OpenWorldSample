using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStateUIController : MonoBehaviour
{
    [SerializeField] MonsterBlackBoard blackBoard;

    private MonsterControllerBT rootMonster;
    public TextMeshProUGUI monsterName;

    public Image HP_Bar;

    public GameObject damageTextPrefab;          
    public Transform damageTextTransform;

    #region Public-Part
    void Start(){
        blackBoard = GetComponentInParent<MonsterBlackBoard>();
        rootMonster = GetComponentInParent<MonsterControllerBT>();
        InitMonsterUIInform();
        InitMonsterUIPosition();
    }
    #endregion

    void Update(){
        LookAtCamera();
    }

    void LookAtCamera(){
        Quaternion targetRotation = Quaternion.LookRotation(Camera.main.transform.position);
        GetComponent<RectTransform>().rotation = targetRotation;
    }

    public void InitMonsterUIInform()
    {
        monsterName.text = blackBoard.monsterData.monsterName;
        blackBoard.OnHPChanged += UpdateMonsterUI;
        GetComponent<RectTransform>().localPosition = Vector2.zero;
        UpdateMonsterUI(blackBoard.monsterData.HP);
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
        transform.GetComponentInParent<MonsterControllerBT>().SetMonsterUI(this);
    }

    void OnDestroy(){
        blackBoard.OnHPChanged -= UpdateMonsterUI;
    }

    void UpdateMonsterUI(int curHP)
    {
        HP_Bar.fillAmount = (float)curHP / blackBoard.monsterData.HP;
    }

    public void ShowDamage(int damage)
    {
        GameObject damageText = Instantiate(damageTextPrefab, damageTextTransform);
        damageText.GetComponent<MonsterDamage>().SetDamage(damage);
        damageText.SetActive(true);
    }
}