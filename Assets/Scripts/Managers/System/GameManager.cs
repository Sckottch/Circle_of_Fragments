using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : ManagerBase<GameManager>
{
    public GameResources gameResources;

    public override IEnumerator Initialize()
    {
        Debug.Log("Inicializando [GameManager]");



        Debug.Log("GameManager Inicializado");
        yield return null;
    }

    public static event Action<CombatData> OnCombatStart;
    public static event Action<CombatEndResult> OnCombatEnd;

    public static void CombatStart(CombatData data)
    {
        Debug.Log("Combate Iniciado");

        OnCombatStart?.Invoke(data);
    }

    public static void CombatEnd(CombatEndResult result)
    {
        Debug.Log($"Combate encerrado. Vitoria: {result.playerWon}");

        OnCombatEnd?.Invoke(result);
    }

}

public class CombatEndResult
{
    public bool playerWon;
}