using System;
using Unity;
using UnityEngine;

[System.Serializable]
public class UnitSprite
{
    private Factions faction;
    private UnitTypes unit;
    private Sprite sprite;
    public UnitSprite(Factions faction,UnitTypes unit, Sprite sprite) 
    {
        this.faction = faction;
        this.unit = unit;
        this.sprite = sprite;

    }

    public bool ByUnitAndFaction(Factions faction, UnitTypes unit)
    {
        if (this.faction == faction && this.unit == unit) 
        {
            return true;
        }
        return false;
    }

    public Sprite GetSprite => sprite;

}

