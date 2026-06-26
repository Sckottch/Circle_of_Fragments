using UnityEngine;

[CreateAssetMenu(fileName = "StackableItem", menuName = "ScriptableObjects/Itens/Stackable")]
public class StackableItem : ItemBase
{
    public int maxStackSize;

    public ItemCategory category;
}
