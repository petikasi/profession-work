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

        public Factions Faction { get; set; }
        public Player Player { get; set; }
        public UnitTypes UnitType { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

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

