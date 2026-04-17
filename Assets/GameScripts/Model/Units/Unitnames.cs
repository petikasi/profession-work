using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.GameScripts.Model.Units
{
    public  class UnitNames
    {
        private static readonly  Dictionary<(Factions, UnitTypes), string> names =
            new()
        {
            //Humans
            { (Factions.Human, UnitTypes.BasicMelee), "Sworn-Brother" },
            { (Factions.Human, UnitTypes.Ranged), "Vigil-Archer" },
            { (Factions.Human, UnitTypes.AdvancedMelee), "Justiciar" },
            { (Factions.Human, UnitTypes.Wizard), "Fate Weaver" },
            { (Factions.Human, UnitTypes.Artillery), "Cataclysm Cannon" },
            { (Factions.Human, UnitTypes.Special), "Archangel of Penance" },
            //Orks
            { (Factions.OrksAndGoblins, UnitTypes.BasicMelee), "Da Boyz" },
            { (Factions.OrksAndGoblins, UnitTypes.Ranged), "Spite-Flinger" },
            { (Factions.OrksAndGoblins, UnitTypes.AdvancedMelee), "Iron-Crusher" },
            { (Factions.OrksAndGoblins, UnitTypes.Wizard), "Boneseer" },
            { (Factions.OrksAndGoblins, UnitTypes.Artillery), "Skull Lobber" },
            { (Factions.OrksAndGoblins, UnitTypes.Special), "The Bloodaxe" },
            //Elves
             { (Factions.Elven, UnitTypes.BasicMelee), "Sentinel" },
            { (Factions.Elven, UnitTypes.Ranged), "Long-Watcher" },
            { (Factions.Elven, UnitTypes.AdvancedMelee), "Oathtaker" },
            { (Factions.Elven, UnitTypes.Wizard), "Archon of Echoes" },
            { (Factions.Elven, UnitTypes.Artillery), "Gaze of the Ancients" },
            { (Factions.Elven, UnitTypes.Special), "The Pale Revenant" },
            //Hell
             { (Factions.Hell, UnitTypes.BasicMelee), "Dread-Claw" },
            { (Factions.Hell, UnitTypes.Ranged), "Void-Stalker" },
            { (Factions.Hell, UnitTypes.AdvancedMelee), "Hell-Knight" },
            { (Factions.Hell, UnitTypes.Wizard), "Calamity-Weaver" },
            { (Factions.Hell, UnitTypes.Artillery), "Soul-Mortar" },
            { (Factions.Hell, UnitTypes.Special), "Seraph of the Void" }
        };


        public static string GetNameForUnit(Factions faction, UnitTypes unittype) 
        {
            string unitname = "";
            try
            {
                unitname = names[(faction, unittype)];


            }
            catch (Exception) 
            {

                Debug.Log($"Cant get name for this unit faction{faction} unittype:{unittype}");
            }

            return unitname;

        }
    }
}
