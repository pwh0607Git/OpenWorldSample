using System.Collections;
using UnityEngine;

//IconBasePrefab에 있는 컨트롤러
public class ItemIconController2 : MonoBehaviour
{
    [SerializeField] ItemData itemData;

    void Start()
    {
        StartCoroutine(SetIcon());
    }

    IEnumerator SetIcon(){
        // itemData가 할당될 때까지 대기
        yield return new WaitUntil(() => itemData != null);

    }
}
