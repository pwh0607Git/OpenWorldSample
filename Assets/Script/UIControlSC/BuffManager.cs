using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;
using static UnityEngine.Rendering.DebugUI.Table;

public class BuffManager : MonoBehaviour
{
    public List<GameObject> activeBuff;

    public GameObject testBuffPrefab;

    public Action OnOffBuffChanged;

    private void Start()
    {
        SortIcons();
    }

    public void SortIcons()
    {
        Vector2 startPosition = new Vector2(0f, 0f);
        Vector2 componentSize = new Vector2(40f, 40f);
        
        int padding = 5;
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

    public void OnBuff(Sprite buffIcon, float duration)
    {
        GameObject newBuff = Instantiate(testBuffPrefab);
        newBuff.GetComponent<Image>().sprite = buffIcon;
        newBuff.transform.SetParent(transform);
        activeBuff.Add(newBuff);

        BuffIconTimer timer = newBuff.GetComponent<BuffIconTimer>();
        if (timer != null)
        {
            timer.OnBuffEnd = OffBuffCallback;
        }

        SortIcons();
    }

    public void OffBuffCallback(GameObject buffEffect)
    {
        activeBuff.Remove(buffEffect);
        SortIcons();
    }
}