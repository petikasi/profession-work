using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameScripts.ViewModel.DeckEditor
{
    [System.Serializable]
    public class FactionSprites
    {
        private Factions faction;
        private Sprite sprite;
        public FactionSprites(Factions faction, Sprite sprite)
        {
            this.faction = faction;
            this.sprite = sprite;

        }

        public bool ByFaction(Factions faction)
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
