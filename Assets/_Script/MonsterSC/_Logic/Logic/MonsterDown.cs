using UnityEngine;

public class MonsterDown : MonoBehaviour
{
    private MonsterBlackBoard blackBoard;
    [SerializeField] private GameObject loots;

    void Start(){
        blackBoard = GetComponentInChildren<MonsterBlackBoard>();
        loots = transform.GetComponentInChildren<MonsterLootHandler>().gameObject;
        loots.SetActive(false);
    }

    public bool IsDownMonster(){
        return !blackBoard.isDown && blackBoard.currentHP <= 0;
    }

    public void DownMonster(){
        blackBoard.animator.SetTrigger("Down");
        blackBoard.isDown = true;
        Invoke("DestroyMonster", 2f);
    }

    private void DestroyMonster(){
        if(loots != null)
        {
            loots.SetActive(true);
            loots.GetComponent<MonsterLootHandler>().ShootLoots();
        }
        Destroy(gameObject);
    }
}
