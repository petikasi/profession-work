using Assets.GameScripts.Model.Game.Enums;
using Assets.GameScripts.ViewModel.Game.UnitSelectorCanvas;
using System;
using System.Xml.Linq;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    public int HealthPoint { get; protected set; }
    public int AttackDamage { get; protected set; }
    public int Defense { get; protected set; }
    public int Shield { get; protected set; }
    public int MovementSpeed { get; protected set; }
    public int Range { get; protected set; }
    public UnitCountpanel MyCardPanel { get; set; }
    public FactionsEnum Faction { get; set; }
    public PlayerEnum Player { get; set; }
    public UnitTypesEnum UnitType { get; set; }
    public int TileX { get; set; }
    public int TileZ { get; set; }

    public bool Already_Moved { get; set; }
    public bool IsRanged { get; set; }

    // Esemény a halál jelzésére (átadja magát az elhalálozott egységet)
    public event Action<BaseUnit> OnUnitDied;

    protected abstract void InitializeStats();

    protected virtual void Awake()
    {
        InitializeStats();
    }

    public virtual void AttackEnemy(BaseUnit target)
    {
        if (target == null) return;

        if (target.Player == this.Player)
        {
            Debug.LogWarning($"{name}: Saját egységet nem támadhatsz meg!");
            return;
        }

        Debug.Log($"{name} megtámadta a következőt: {target.name} (Sebzés: {AttackDamage})");
        target.TakeDamage(AttackDamage);
    }

    public virtual void TakeDamage(int amount)
    {
        // Javítva: amount - Defense
        int rawDamage = Mathf.Max(0, amount - Defense);

        if (Shield > 0)
        {
            if (Shield >= rawDamage)
            {
                Shield -= rawDamage;
                rawDamage = 0;
            }
            else
            {
                rawDamage -= Shield;
                Shield = 0;
            }
        }

        HealthPoint -= rawDamage;
        Debug.Log($"{name} sebződött: {rawDamage}. Maradt HP: {HealthPoint}, Shield: {Shield}");

        if (HealthPoint <= 0)
        {
            Die();
        }
    }

    public virtual void Heal(int amount)
    {
        HealthPoint += amount;
    }

    public virtual void AddShield(int amount)
    {
        Shield += amount;
    }

    public virtual void Die()
    {
        Debug.Log($"{name} meghalt!");

        // Kiváltjuk az eseményt a feliratkozók felé (pl. BoardLayout)
        OnUnitDied?.Invoke(this);
    }
}