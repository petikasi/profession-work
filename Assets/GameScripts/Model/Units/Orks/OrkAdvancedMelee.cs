using UnityEngine;
public class OrkAdvancedMelee : BaseUnit
{
    protected override void InitializeStats()
    {
        HealthPoint = 20;
        AttackDamage = 4;
        Defense = 2;
        Shield = 5;
        MovementSpeed = 3;
        Range = 1;
    }

    protected override void Die()
    {
        Debug.Log("Human Melee died");
    }

}
