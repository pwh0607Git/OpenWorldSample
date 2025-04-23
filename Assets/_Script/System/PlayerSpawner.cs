using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] PlayerController1 playerPrefab;
    [SerializeField] List<AbilityData> datas;

    void Start(){
        PlayerController1 player = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        player.SetAbility(datas);
    }
}