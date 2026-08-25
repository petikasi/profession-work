using UnityEngine;

public class ElvenWizard : BaseUnit
{
    protected override void InitializeStats()
    {
        HealthPoint = 50;
        AttackDamage = 6;
        Defense = 2;
        Shield = 10;
        MovementSpeed = 5;
        Already_Moved = false;
        Range = 15;
    }

    protected override void Die()
    {
        Debug.Log("Human Melee died");
    }




}
