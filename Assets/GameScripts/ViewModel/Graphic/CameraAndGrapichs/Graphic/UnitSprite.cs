using System;
using Unity;
using UnityEngine;

namespace Assets.GameScripts.ViewModel.CameraAndGrapichs.Grapic
{
    [System.Serializable]
    public class UnitSprite
    {
        private readonly FactionsEnum faction;
        private readonly UnitTypesEnum unit;
        private readonly Sprite sprite;
        public UnitSprite(FactionsEnum faction, UnitTypesEnum unit, Sprite sprite)
        {
            this.faction = faction;
            this.unit = unit;
            this.sprite = sprite;

        }

        public bool ByUnitAndFaction(FactionsEnum faction, UnitTypesEnum unit)
        {
            if (this.faction == faction && this.unit == unit)
            {
                return true;
            }
            return false;
        }

        public Sprite GetSprite => sprite;

    }

}