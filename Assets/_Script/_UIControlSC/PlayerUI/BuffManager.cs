using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void OnBuffItem(Consumable itemData, float duration)
    {
        GameObject existingBuff = CheckExistingBuff(itemData.icon);
        if (existingBuff != null)
        {
            BuffIconTimer timer = existingBuff.GetComponent<BuffIconTimer>();
            timer.StartTimer(duration);

            return;
        }

        GameObject newBuff = Instantiate(testBuffPrefab);
        newBuff.GetComponent<Image>().sprite = itemData.icon;
        newBuff.transform.SetParent(transform);
        activeBuff.Add(newBuff);

        BuffIconTimer newTimer = newBuff.GetComponent<BuffIconTimer>();
        if (newTimer != null)
        {
            newTimer.OnBuffEnd = OffBuffCallback;
        }

        newTimer.StartTimer(duration);

        SortIcons();
    }

    public void OnBuffSkill()
    {

    }

    public void OffBuffCallback(GameObject buffEffect)
    {
        activeBuff.Remove(buffEffect);
        SortIcons();
    }

    public GameObject CheckExistingBuff(Sprite buffIcon)
    {
        foreach (var buff in activeBuff)
        {
            BuffIconTimer timer = buff.GetComponent<BuffIconTimer>();
            
            if(timer.GetComponent<Image>().sprite == buffIcon)
            {
                return buff;
            }
        }
        return null;
    }
}