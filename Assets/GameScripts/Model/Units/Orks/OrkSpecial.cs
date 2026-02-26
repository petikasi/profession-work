using UnityEngine;

public  class OrkSpecial : BaseUnit
{
    protected override void InitializeStats()
    {
        HealthPoint = 200;
        AttackDamage = 20;
        Defense = 10;
        Shield = 50;
        MovementSpeed = 10;
        Range =1;
    }

    protected override void Die()
    {
        Debug.Log("Human Melee died");
    }




}
