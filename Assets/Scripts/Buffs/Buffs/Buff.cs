using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : ScriptableObject
{
    [Space(10)]
    [Header("Buff Info")]
    public string buffName;
    [TextArea] public string description;

    [Space(10)]
    [Header("Display Info")]
    public string displayName;

    [Space(10)]
    [Header("Buff Settings")]
    public int duration; // Duration in turns

    [Tooltip("Maximum number of stacks this buff can have. 1 for non-stackable.")]
    public int maxStacks; 
    public BuffCategory category;

    [Space(10)]
    public bool isDebuff; 
    public BuffActionTime actionTime; // When the buff apply its effect(if it has any)

    #region Private Fields

    protected List<BuffStack> stacks;
    public Unit Caster { get; protected set; }
    public Unit Target { get; protected set; }

    #endregion

    public abstract void OnApply(Unit target, Unit caster); 

    public abstract void ApplyEffect(Unit target);

    public abstract void OnRemove(Unit target);

    protected abstract void OnStackChange();

    public void Add()
    {
        stacks ??= new();

        BuffStack newStack = new BuffStack
        {
            buff = this,
            duration = duration
        };

        if (stacks.Count < maxStacks)
        {
            stacks.Add(newStack);
        }
        else
        {
            // Substitui o stack com menor duração pelo novo stack
            int minIndex = 0;
            int minDuration = stacks[0].duration;

            for (int i = 1; i < stacks.Count; i++)
            {
                if (stacks[i].duration < minDuration)
                {
                    minDuration = stacks[i].duration;
                    minIndex = i;
                }
            }

            stacks[minIndex] = newStack;
        }
    }

    public void Tick()
    {
        if (stacks == null)
            return;


        foreach (BuffStack stack in stacks)
        {
            stack.duration--;
        }
    }

    public void RemoveExpiredStacks()
    {
        if (stacks == null) return;

        int initialCount = stacks.Count;

        stacks.RemoveAll(stack => stack.duration <= 0);

        int removedCount = initialCount - stacks.Count;

        if (stacks.Count == 0)
        {
            duration = 0;
            return;
        }

        if (removedCount > 0 && Target != null)
        {
            OnStackChange();
        }
    }

}

[System.Serializable]
public class BuffStack
{
    public Buff buff;
    public int duration; // Remaining turns
}