using UnityEngine;

public abstract class ItemBase : ScriptableObject
{
    public string ItemID { get; protected set; } = System.Guid.NewGuid().ToString();

    public string itemName;
    public Sprite icon;
}
