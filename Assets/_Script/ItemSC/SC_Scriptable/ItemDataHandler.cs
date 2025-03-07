using UnityEngine;

public abstract class ItemHandler : MonoBehaviour
{
    public abstract ItemData GetData { get; }
    public abstract void Init(ItemData itemData);
    public abstract void Use();
}