using System.Collections;
using UnityEngine;

public class CombatManager : SingletonMonoBehaviour<CombatManager>
{
    [Header("Referencias")]
    [SerializeField] private CombatSetup combatSetup;

    [SerializeField] private CombatUIManager combatUIManager;

    public CombatContext Context { get; private set; }
    public CombatSetup CombatSetup => combatSetup;
    public CombatUIManager CombatUIManager => combatUIManager;

    private CombatStateMachine combatStateMachine = new ();
    public CombatState CurrentState { get; private set; } = CombatState.Idle;
    public CombatData CurrentCombatData { get; private set; }
    public CombatEvents Events { get; private set; }
    private CombatListener listener;
    public CombatInputHandler Inputs { get; private set; }
    
    //States
    private IdleCombatState idleState = new ();
    private WaveStartCombatState waveStartState = new ();
    private BattleCombatState battleState = new ();
    private WaveEndCombatState waveEndState = new ();
    private VictoryCombatState victoryState = new ();
    private DefeatCombatState defeatState = new ();

    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.OnCombatStart += StartCombat;
        GameManager.Instance.OnCombatEnd += EndCombat;
        RandomManager.Init();
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCombatStart -= StartCombat;
        GameManager.Instance.OnCombatEnd -= EndCombat;
    }

    public void ChangeCombatState(CombatState state)
    {
        CurrentState = state;

        switch (state)
        {
            case CombatState.Idle:
                combatStateMachine.ChangeState(idleState);
                break;

            case CombatState.WaveStart:
                combatStateMachine.ChangeState(waveStartState);
                break;

            case CombatState.Battle:
                combatStateMachine.ChangeState(battleState);
                break;

            case CombatState.WaveEnd:
                combatStateMachine.ChangeState(waveEndState);
                break;

            case CombatState.Victory:
                combatStateMachine.ChangeState(victoryState);
                break;

            case CombatState.Defeat:
                combatStateMachine.ChangeState(defeatState);
                break;            

            default: break;
        }
    }

    private void StartCombat(CombatData data)
    {
        Events = new CombatEvents();
        listener = new CombatListener(Events);
        combatUIManager.SubscribeUIEvents();
        Inputs = new CombatInputHandler();
        CurrentCombatData = data;
        ChangeCombatState(CombatState.Idle);
    }

    private void EndCombat(CombatEndResult result)
    {
        listener.Dispose();
        Inputs.Dispose();
        combatUIManager.UnsubscribeUIEvents();
        Events = null;
    }

    public void StartRoutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    public void SetCurrentContext(CombatContext context) => Context = context;
}