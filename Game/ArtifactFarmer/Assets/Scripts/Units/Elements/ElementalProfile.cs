using System.Collections.Generic;

public class ElementalProfile
{
    public Element Element { get; private set; }
    private List<Element> weakness;
    private Element resistance;

    public ElementalProfile(Element element, List<Element> weakness, Element resistance)
    {
        Element = element;
        this.weakness = weakness;
        this.resistance = resistance;
    }

    public ElementalMatchupResult GetElementalMatchup(Element attackerElement)
    {
        ElementalMatchupResult result = new()
        {
            bonus = new SpecialStats()
        };

        if (IsSpecialElement(attackerElement))
        {
            result.bonus.critRate = 5f;
            result.bonus.damageBonus = 10f;
            result.bonus.effectChance = 5f;
            return result;
        }

        if (weakness.Contains(attackerElement))
        {
            result.bonus.critRate = 10f;
            result.bonus.damageBonus = 20f;
            result.bonus.effectChance = 10f;
            result.isWeakness = true;
            return result;           
        }

        if (attackerElement == resistance)
        {
            result.bonus.critRate = -5f;
            result.bonus.damageBonus = -10f;
            result.bonus.effectChance = -5f;
            result.isResistance = true;
        }
        
        return result;
    }
    
    
    private bool IsSpecialElement(Element element)
    {
        return element switch
        {
            Element.Light => true,
            Element.Dark => true,
            Element.Lightning => true,

            _ => false
        };
    }
}

public struct ElementalMatchupResult
{
    public SpecialStats bonus;
    public bool isWeakness;
    public bool isResistance;
}