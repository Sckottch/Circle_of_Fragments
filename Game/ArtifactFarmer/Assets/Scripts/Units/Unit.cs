using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitView))]
[DisallowMultipleComponent]
public abstract class Unit : MonoBehaviour
{
    public string UnitID { get; protected set; } = Guid.NewGuid().ToString();

    public string UnitName { get; protected set; }

    protected Stats baseStats;
    protected SpecialStats baseSpecialStats;
    protected List<Skill> skills = new ();
    public ElementalProfile ElementalProfile { get; protected set; }

    protected float lastHealth;

    public float CurrentHealth {get; protected set; }
    public float CurrentMana { get; protected set; }

    public List<Buff> ActiveBuffs { get; private set; } = new ();

    public abstract bool IsPlayer { get; }

    public UnitBaseData UnitData { get; protected set; }
    public ModifierSystem ModifierSystem { get; protected set; } = new();
    protected UnitView view;

    //Events
    public event Action OnHealthChanged;
    public event Action<Unit> OnDeath;
    public event Action OnManaChanged;
    public event Action<Unit> OnMaxHealthChanged;
    public event Action<Unit, float> OnHit;
    // public event Action<Unit, Buff, Unit> OnBuffApplied; TODO: avaliar se os eventos de buff ficam no unit, e se não for o caso movê-los ao local correto
    // public event Action<Unit, Buff> OnBuffRemoved; 

    protected void Awake()
    {
        view = GetComponent<UnitView>();
    }

    public abstract void Initialize(UnitBaseData unitData, int experience = 0);

    public abstract Skill GetSkill(ActionType type);

    public virtual Stats GetStats()
    {
        return baseStats;
    }

    public virtual SpecialStats GetSpecialStats()
    {
        return baseSpecialStats;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        HealthChanged();
        OnHit?.Invoke(this, damage);

        if (CurrentHealth <= 0)
        {
            Death(this);
        }
    }

    public bool ConsumeMana(float amount)
    {
        if (CurrentMana < amount)
        {
            return false;
        }

        CurrentMana -= amount;
        ManaChanged();
        return true;
    }

    public void GainMana(float amount)
    {
        CurrentMana += amount;
        CurrentMana = Mathf.Min(CurrentMana, GetSpecialStats().mana);
        ManaChanged();
    }

    public void Heal(float amount)
    {
        CurrentHealth += amount;
        CurrentHealth = Mathf.Min(CurrentHealth, GetStats().health);
        HealthChanged();
    }

    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }

    public void HealthChanged()
    {
        OnHealthChanged?.Invoke();
    }

    public void ManaChanged()
    {
        OnManaChanged?.Invoke();
    }

    public void Death(Unit unit)
    {
        OnDeath?.Invoke(unit);
        CombatManager.Instance.Events.UnitDied(this);
        gameObject.SetActive(false);
    }

    public void ModifyActionValue(float percentage)
    {
        CombatManager.Instance.Events.AVModified(this, percentage);
    }

    public void ApplyBuff(Buff buff, Unit caster)
    {
        //OnBuffApplied?.Invoke(this, buff, caster);
    }

    public void RemoveBuff(Buff buff)
    {
        //OnBuffRemoved?.Invoke(this, buff);
    }

    public void SpeedChanged()
    {
        CombatManager.Instance.Events.SpeedChanged(this);
    }

    public void MaxHealthChanged()
    {
        UpdateCurrentHealth();
        OnMaxHealthChanged?.Invoke(this);
    }

    private void UpdateCurrentHealth()
    {
        float currentHealthPercentage = CurrentHealth / lastHealth;
        CurrentHealth = GetStats().health * currentHealthPercentage;
    }

    public bool CanAct()
    {
        return IsAlive();
    }
}