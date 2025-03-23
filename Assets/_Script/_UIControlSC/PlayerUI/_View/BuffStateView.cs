using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuffStateView : MonoBehaviour
{
    public List<BuffIconTimer> activeBuffIcons;

    public GameObject buffIconPrefab;

    public UnityAction<IStateEffect> OnBuffEnd;
    public UnityAction<IStateEffect> OnBuffStart;

    public void OnBuff(IStateEffect effect)
    {
        BuffIconTimer existingBuff = CheckExistingBuff(effect.GetData().icon);
        if (existingBuff != null)
        {
            existingBuff.StartTimer(effect.GetData().duration);
            return;
        }

        BuffIconTimer newBuff = Instantiate(buffIconPrefab, transform).GetComponent<BuffIconTimer>();
        newBuff.InitBuff(effect);
        activeBuffIcons.Add(newBuff);
        if (newBuff != null) newBuff.OnBuffEnd += OnBuffEndCallback;
        OnBuffStart?.Invoke(effect);
    }

    public void OnBuffEndCallback(BuffIconTimer buff)
    {
        activeBuffIcons.Remove(buff);
        OnBuffEnd?.Invoke(buff.effect);
    }

    public BuffIconTimer CheckExistingBuff(Sprite buffIcon)
    {
        foreach (var buff in activeBuffIcons)
        {   
            if(buff.GetComponent<Image>().sprite == buffIcon) return buff;
        }
        return null;
    }
}