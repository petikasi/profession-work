using UnityEngine;

public class ElvenRanged : BaseUnit
{
    protected override void InitializeStats()
    {
        HealthPoint = 10;
        AttackDamage = 5;
        Defense = 2;
        Shield = 5;
        MovementSpeed = 3;
        Range = 20;
        Already_Moved = false;
    }

    protected override void Die()
    {
        Debug.Log("Human Melee died");
    }




}
