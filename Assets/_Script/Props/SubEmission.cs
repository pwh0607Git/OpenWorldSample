using UnityEngine;

public class SubEmission : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        Debug.Log("Sub-Emission Trigger 발생!");
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("플레이어 격추! => Sub");
            // other.gameObject.GetComponent<PlayerController>().PlayerTakeDamage(15);
        }
        Destroy(gameObject);
    }
}
