using UnityEngine;

public abstract class ItemDataHandler : MonoBehaviour
{
    public abstract ItemData GetItem { get; }
    public abstract void Init(ItemData itemData);
}