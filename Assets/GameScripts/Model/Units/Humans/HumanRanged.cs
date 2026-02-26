using UnityEngine;

public class HumanRanged : BaseUnit
{
    protected override void InitializeStats()
    {
        HealthPoint = 10;
        AttackDamage = 5;
        Defense = 2;
        Shield = 5;
        MovementSpeed = 3;
        Range = 20;
    }

    protected override void Die()
    {
        Debug.Log("Human Melee died");
    }




}
