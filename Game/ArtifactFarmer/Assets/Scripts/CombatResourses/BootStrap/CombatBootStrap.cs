using UnityEngine;
public class CombatBootStrap : MonoBehaviour
{
    private void Start()
    {
        GameResources resources = GameManager.Instance.gameResources;
        CombatData data = new (new PlayerCombatData(resources.playerUnits), resources.combatData);
        
        GameManager.Instance.CombatStart(data);
    }
}