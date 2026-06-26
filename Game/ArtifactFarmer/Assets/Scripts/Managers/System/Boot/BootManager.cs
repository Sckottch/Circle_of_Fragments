using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BootManager : MonoBehaviour
{
    [Space(10)]
    [Header("GameSystemsRoot Prefab")]
    [SerializeField] private GameObject gameSystemsRootPrefab;

    private List<IInitializable> managers = new();

    private IEnumerator Start()
    {
        Debug.Log("Iniciando Sequencia de Boot");

        if (GameSystemRoot.Instance == null)
        {
            if (gameSystemsRootPrefab == null)
            {
                Debug.LogError("GameSystemsRoot Prefab não atribuido no BootManager!");
                yield break;
            }

            Instantiate(gameSystemsRootPrefab);
            Debug.Log("GameSystemsRoot Instanciado");

            yield return null;
        }

        managers.Clear();
        
        MonoBehaviour[] foundManagers = GameSystemRoot.Instance.GetComponentsInChildren<MonoBehaviour>();

        foreach (MonoBehaviour mb in foundManagers)
        {
            if (mb is IInitializable initializableManager)
            {
                managers.Add(initializableManager);
                Debug.Log($"Gerenciador Encontrado para Inicialização: {mb.GetType().Name}");
            }
        }

        List<IInitializable> sortedManagers = SortManagers(managers);

        foreach (IInitializable manager in sortedManagers)
        {
            yield return manager.Initialize();
        }

        Debug.Log("Sequencia de Boot Concluida, Mudando para a cena do menu.");

        GameManager.Instance.GoToMainMenu();
    }

    private List<IInitializable> SortManagers(List<IInitializable> managersToSort)
    {
        List<IInitializable> sortedManagers = new();

        void AddManager<T>() where T : IInitializable
        {
            IInitializable manager = managersToSort.FirstOrDefault(m => m is T);
            if (manager != null && !sortedManagers.Contains(manager))
            {
                sortedManagers.Add(manager);
            }
        }

        AddManager<SaveManager>();
        AddManager<SettingsManager>();
        AddManager<LocalizationManager>();
        AddManager<AudioManager>();
        AddManager<ProfileManager>();
        AddManager<PlayerManager>();
        AddManager<SceneLoader>();
        AddManager<GameManager>();

        foreach (IInitializable manager in managersToSort)
        {
            if (!sortedManagers.Contains(manager))
            {
                sortedManagers.Add(manager);
            }
        }

        return sortedManagers;
    }

}
