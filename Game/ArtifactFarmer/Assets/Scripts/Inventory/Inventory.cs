using System;
using System.Collections.Generic;

public class Inventory
{  
    public Dictionary<StackableItem, int> StackableItens { get; private set;  }

    public Inventory()
    {
        StackableItens = new();
    }

    public void AddStackableItem(StackableItem item, int quantity)
    {
        if (quantity <= 0)
            return;

        if (quantity > item.maxStackSize)
            quantity = item.maxStackSize;

        if (StackableItens.ContainsKey(item))
        {
            StackableItens[item] = Math.Min(StackableItens[item] + quantity, item.maxStackSize);
        }
        else
        {
            StackableItens.Add(item, quantity);
        }
    }
}
