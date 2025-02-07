using UnityEngine;

public class SubEmissionMaker : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    void Awake()
    {
        MakeSubEmission();
    }

    void MakeSubEmission(){
        Debug.Log("서브 생성!@");
        int ran = Random.Range(3,7);
        for(int i=0;i<ran;i++){
            float ranFloat = Random.Range(0.2f, 0.5f); 
            GameObject instance = Instantiate(prefab, transform);
            instance.transform.localScale = Vector3.one * ranFloat;
        }
    }
}
