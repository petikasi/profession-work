using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameScripts.ViewModel.CameraAndGrapichs.Grapic
{
    [System.Serializable]
    public class FactionSprites
    {
        private readonly FactionsEnum faction;
        private readonly Sprite sprite;
        public FactionSprites(FactionsEnum faction, Sprite sprite)
        {
            this.faction = faction;
            this.sprite = sprite;

        }

        public bool ByFaction(FactionsEnum faction)
        {
            if (this.faction == faction)
            {
                return true;
            }
            return false;
        }

        public Sprite GetSprite => sprite;
    }
}
