using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class BuffManager : MonoBehaviour
{
    //아이콘 정렬
    public List<GameObject> activeBuff;

    private void Start()
    {
        //activeBuff = new List<GameObject>();
        SortIcons();
    }

    public void SortIcons()
    {
        Debug.Log("버프 아이콘 정렬 수행");
        int padding = 5;

        Vector2 startPosition = new Vector2(0f, 0f);
        Vector2 componentSize = new Vector2(40f, 40f);
        
        int i = 0;

        foreach(var buffIcon in activeBuff)
        {
            RectTransform rectTransform = buffIcon.GetComponent<RectTransform>();
            rectTransform.sizeDelta = componentSize;
            rectTransform.localScale = Vector2.one;
            rectTransform.anchoredPosition = new Vector2(startPosition.x + i * (componentSize.x + padding), startPosition.y);
            i++;
        }
    }

    public void OnBuff(GameObject buffEffect)
    {
        activeBuff.Add(buffEffect);
        SortIcons();
    }

    public void OffBuff(GameObject buffEffect)
    {
        activeBuff.Remove(buffEffect);
        SortIcons();
    }
}