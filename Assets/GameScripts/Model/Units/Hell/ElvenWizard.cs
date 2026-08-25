using UnityEngine;

public class HellWizard : BaseUnit
{
    protected override void InitializeStats()
    {
        HealthPoint = 50;
        AttackDamage = 6;
        Defense = 2;
        Shield = 10;
        Already_Moved = false;
        MovementSpeed = 5;
        Range = 15;
    }

    protected override void Die()
    {
        Debug.Log("Human Melee died");
    }




}
