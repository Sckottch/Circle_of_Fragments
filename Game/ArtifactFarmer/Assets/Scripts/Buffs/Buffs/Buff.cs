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

    protected List<BuffStack> stacks;
    public Unit Caster { get; protected set; }
    public Unit Target { get; protected set; }
    public bool IsExpired() => stacks.Count == 0;

    public abstract void OnApply(Unit target, Unit caster); 

    public abstract void ApplyEffect();

    public abstract void OnRemove();

    protected abstract void OnStackChange();

    protected void Add()
    {
        stacks ??= new List<BuffStack>();

        BuffStack newStack = new BuffStack
        {
            duration = duration
        };

        if (stacks.Count < maxStacks)
        {
            stacks.Add(newStack);
        }
        else
        {
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
        if (stacks == null) return;

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

        if (removedCount > 0 && Target != null)
        {
            OnStackChange();
        }
    }
}

[System.Serializable]
public class BuffStack
{
    public int duration; // Remaining turns
}