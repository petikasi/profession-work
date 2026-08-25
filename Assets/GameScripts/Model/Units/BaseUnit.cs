using Assets.GameScripts.ViewModel.Game.UnitSelectorCanvas;
using Assets.GameScripts.Model.Game.Enums;
using UnityEngine;


    public abstract class BaseUnit : MonoBehaviour
    {
        public int HealthPoint { get; protected set; }

        public int AttackDamage { get; protected set; }
        public int Defense { get; protected set; }
        public int Shield { get; protected set; }
        public int MovementSpeed { get; protected set; }
        public int Range { get; protected set; }

        public int Initiate { get; protected set; }
        public UnitCountpanel MyCardPanel { get; set; }
        public FactionsEnum Faction { get; set; }
        public PlayerEnum Player { get; set; }
        public UnitTypesEnum UnitType { get; set; }
        public int TileX { get; set; }
        public int TileZ { get; set; }

        public bool Already_Moved { get; set; }

    protected abstract void InitializeStats();

        protected virtual void Awake()
        {
            InitializeStats();
        }

        public virtual void TakeDamage(int amount)
        {

            int damage = Mathf.Max(0, amount - Defense - Shield);
            HealthPoint -= damage;

            if (HealthPoint <= 0)
                Die();
        }

        public virtual void Heal(int amount)
        {
            HealthPoint = HealthPoint + amount;
        }

        public virtual void AddShield(int amount)
        {
            Shield += amount;
        }

        protected abstract void Die();



    }

