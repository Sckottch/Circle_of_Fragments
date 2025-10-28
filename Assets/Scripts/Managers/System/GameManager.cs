using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : ManagerBase<GameManager>
{
    public GameMode CurrentGameMode { get; private set; } = GameMode.None;
    public GameResources gameResources;

    private SceneLoader sceneManager;

    public override IEnumerator Initialize()
    {
        Debug.Log("Inicializando [GameManager]");

        sceneManager = SceneLoader.Instance;

        Debug.Log("GameManager Inicializado");
        yield return null;
    }

    #region Game Mode Management

    private void SetGameScene(GameMode mode)
    {
        if (mode == CurrentGameMode)
        {
            Debug.LogWarning($"O modo de jogo já está definido como '{mode}'.");
            return;
        }

        //EndCurrentMode(CurrentGameMode); não implementado ainda para testes

        CurrentGameMode = mode;

        switch (mode)
        {
            case GameMode.MainMenu:
                StartCoroutine(sceneManager.LoadScene("MainMenu"));
                break;

            case GameMode.GameLobby:
                StartCoroutine(sceneManager.LoadScene("GameLobby"));
                break;

            case GameMode.Expedition:
                StartCoroutine(sceneManager.LoadScene("ExpeditionScene"));
                break;

            case GameMode.Combat:
                StartCoroutine(sceneManager.LoadScene("CombatScene"));
                break;

        }
    }

    public void GoToMainMenu()
    {
        SetGameScene(GameMode.MainMenu);
    }

    #endregion

    #region Combat Functions

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

    #endregion
}

public class CombatEndResult
{
    public bool playerWon;
}