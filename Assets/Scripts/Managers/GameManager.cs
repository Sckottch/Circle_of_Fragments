using System;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public GameResources gameResources;

    #region Singleton

    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    

    #region Temp Setup Region
    //Região para colocar funções de teste para fase atual do projeto
    //essas funções serão movidas para diferentes classes no futuro porem serão necessarias para testes iniciais

    private CombatData combatData;

    private void Start()
    {
        RandomManager.Init();
        combatData = new CombatData
        (
            new(gameResources.playerUnits),
            gameResources.combatData
        );
        CombatStart(combatData);
    }

    #endregion

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

public struct CombatEndResult
{
    public bool playerWon;
}