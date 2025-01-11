using TMPro;
using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    private TextMeshPro damageText;

    public float speed = 2.0f;
    public float lifeTime = 1.0f;
    private float timer;

    private void Start()
    {
        timer = 0.0f;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        damageText = GetComponentInChildren<TextMeshPro>();
    }

    private void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;

        timer += Time.deltaTime;
        FadeOut();
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(int damage)
    {
        GetComponent<TextMeshPro>().text = damage.ToString();
    }

    void FadeOut(){
        damageText.alpha = Mathf.Lerp(1f,0f,timer/lifeTime);
    }
}