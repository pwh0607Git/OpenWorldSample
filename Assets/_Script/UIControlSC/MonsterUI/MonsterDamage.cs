using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    private TextMeshProUGUI damageTXT;

    private void Start()
    {
        damageTXT = GetComponent<TextMeshProUGUI>();   
    }

    public float speed = 2.0f;
    public float lifeTime = 1.0f;
    private float timer;

    private void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(int damage)
    {
        damageTXT.text = damage.ToString();
    }

    //오브젝트 풀링 형으로 출력 예정.
    private void OnEnable()
    {
        timer = 0.0f;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }
}
