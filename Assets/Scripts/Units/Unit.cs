using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[DisallowMultipleComponent]
public abstract class Unit : MonoBehaviour
{
    public string UnitName { get; protected set; }

    protected Stats baseStats;
    protected SpecialStats baseSpecialStats;

    protected float lastHealth;

    public float CurrentHealth {get; protected set; }
    public float CurrentMana { get; protected set; }

    public List<Buff> ActiveBuffs { get; private set; } = new List<Buff>();

    public abstract bool IsPlayer { get; }

    public UnitBaseData UnitData { get; protected set; }
    public ModifierSystem ModifierSystem { get; protected set; } = new();

    protected SpriteRenderer spriteRenderer;

    //Events
    public event Action OnHealthChanged;
    public event Action<Unit> OnDeath;
    public event Action OnManaChanged;
    public event Action<Unit> OnUnitSelected;
    public event Action<Unit, float> OnAVModify;
    public event Action<Unit, Buff, Unit> OnBuffApplied;
    public event Action<Unit, Buff> OnBuffRemoved;
    public event Action<Unit> OnSpeedChanged;
    public event Action<Unit> OnMaxHealthChanged;
    public event Action<Unit, float> OnHit;
    public event Action<Unit, Unit, List<Unit>, Skill> OnSkillUsed;
    public event Action<Unit, SkillResult> OnSkillResult;

    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public abstract void Initialize(UnitBaseData unitData, int experience = 0);

    public abstract bool TurnAction(Unit mainTarget, List<Unit> allTargets, ActionType action);

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
    }

    public void ModifyActionValue(float percentage)
    {
        OnAVModify?.Invoke(this, percentage);
    }

    public void ApplyBuff(Buff buff, Unit caster)
    {
        OnBuffApplied?.Invoke(this, buff, caster);
    }

    public void RemoveBuff(Buff buff)
    {
        OnBuffRemoved?.Invoke(this, buff);
    }

    public void SpeedChanged()
    {
        OnSpeedChanged?.Invoke(this);
    }

    public void SkillUsed(Unit caster, Unit mainTarget, List<Unit> allTargets, Skill skill)
    {
        OnSkillUsed?.Invoke(caster, mainTarget, allTargets, skill);
    }

    public void CallSkillResult(SkillResult result)
    {
        OnSkillResult?.Invoke(this, result);
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

    public void SetMaterial(Material material)
    {
        spriteRenderer.material = material;
    }

    public bool CanAct()
    {
        return IsAlive();
    }

    private void OnMouseDown()
    {
        if (CombatManager.IsWaitingTarget && IsAlive())
        {
            OnUnitSelected?.Invoke(this);
        }
    }

}