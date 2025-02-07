using UnityEngine;

public class SubEmission : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("플레이어 격추! => Sub");
            // other.gameObject.GetComponent<PlayerController>().PlayerTakeDamage(15);
        }
        Destroy(gameObject);
    }
}
