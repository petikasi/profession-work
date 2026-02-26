using UnityEngine;

public  class HumanArtillery : BaseUnit
{
    protected override void InitializeStats()
    {
        HealthPoint = 40;
        AttackDamage = 10;
        Defense = 5;
        Shield = 10;
        MovementSpeed = 2;
        Range = 40;
    }

    protected override void Die()
    {
        Debug.Log("Human Melee died");
    }


}
