using UnityEngine;

public  class HellSpecial : BaseUnit
{
    protected override void InitializeStats()
    {
        HealthPoint = 200;
        AttackDamage = 20;
        Defense = 10;
        Already_Moved = false;
        Shield = 50;
        MovementSpeed = 10;
        Range =1;
    }

    protected override void Die()
    {
        Debug.Log("Human Melee died");
    }




}
