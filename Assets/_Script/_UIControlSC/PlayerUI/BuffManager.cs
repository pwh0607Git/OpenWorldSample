using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    public List<BuffIconTimer> activeBuffIcons;

    public GameObject testBuffPrefab;

    public UnityAction OnBuffEnd;
    public UnityAction OnBuffStart;
    

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

        foreach(var buffIcon in activeBuffIcons)
        {
            RectTransform rectTransform = buffIcon.GetComponent<RectTransform>();
            rectTransform.sizeDelta = componentSize;
            rectTransform.localScale = Vector2.one;
            rectTransform.anchoredPosition = new Vector2(startPosition.x + i * (componentSize.x + padding), startPosition.y);
            i++;
        }
    }

    public void OnBuffItem(ConsumableData itemData, float duration)
    {
        BuffIconTimer existingBuff = CheckExistingBuff(itemData.icon);
        if (existingBuff != null)
        {
            existingBuff.StartTimer(duration);

            return;
        }

        BuffIconTimer newBuff = Instantiate(testBuffPrefab, transform).GetComponent<BuffIconTimer>().InitBuff(itemjData);
        activeBuffIcons.Add(newBuff);
        if (newBuff != null) newBuff.OnBuffEnd = OnBuffEndCallback;

        SortIcons();
    }

    public void OnBuffEndCallback(BuffIconTimer buffEffect)
    {
        activeBuffIcons.Remove(buffEffect);
        SortIcons();
    }

    public BuffIconTimer CheckExistingBuff(Sprite buffIcon)
    {
        foreach (var buff in activeBuffIcons)
        {   
            if(buff.GetComponent<Image>().sprite == buffIcon)
            {
                return buff;
            }
        }
        return null;
    }
}