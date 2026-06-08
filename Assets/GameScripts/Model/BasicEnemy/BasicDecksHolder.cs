using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameScripts.Model.BasicEnemy
{
    public class BasicDecksHolder
    {
        private static readonly Dictionary<(FactionsEnum, UnitTypesEnum), string> names =
      new()
  {
            //Humans
            { (FactionsEnum.Human, UnitTypesEnum.BasicMelee), "Sworn-Brother" },
            { (FactionsEnum.Human, UnitTypesEnum.Ranged), "Vigil-Archer" },
            { (FactionsEnum.Human, UnitTypesEnum.AdvancedMelee), "Justiciar" },
            { (FactionsEnum.Human, UnitTypesEnum.Wizard), "Fate Weaver" },
            { (FactionsEnum.Human, UnitTypesEnum.Artillery), "Cataclysm Cannon" },
            { (FactionsEnum.Human, UnitTypesEnum.Special), "Archangel of Penance" },
            //Orks
            { (FactionsEnum.Orks, UnitTypesEnum.BasicMelee), "Da Boyz" },
            { (FactionsEnum.Orks, UnitTypesEnum.Ranged), "Spite-Flinger" },
            { (FactionsEnum.Orks, UnitTypesEnum.AdvancedMelee), "Iron-Crusher" },
            { (FactionsEnum.Orks, UnitTypesEnum.Wizard), "Boneseer" },
            { (FactionsEnum.Orks, UnitTypesEnum.Artillery), "Skull Lobber" },
            { (FactionsEnum.Orks, UnitTypesEnum.Special), "The Bloodaxe" },
            //Elves
             { (FactionsEnum.Elven, UnitTypesEnum.BasicMelee), "Sentinel" },
            { (FactionsEnum.Elven, UnitTypesEnum.Ranged), "Long-Watcher" },
            { (FactionsEnum.Elven, UnitTypesEnum.AdvancedMelee), "Oathtaker" },
            { (FactionsEnum.Elven, UnitTypesEnum.Wizard), "Archon of Echoes" },
            { (FactionsEnum.Elven, UnitTypesEnum.Artillery), "Gaze of the Ancients" },
            { (FactionsEnum.Elven, UnitTypesEnum.Special), "The Pale Revenant" },
            //Hell
             { (FactionsEnum.Hell, UnitTypesEnum.BasicMelee), "Dread-Claw" },
            { (FactionsEnum.Hell, UnitTypesEnum.Ranged), "Void-Stalker" },
            { (FactionsEnum.Hell, UnitTypesEnum.AdvancedMelee), "Hell-Knight" },
            { (FactionsEnum.Hell, UnitTypesEnum.Wizard), "Calamity-Weaver" },
            { (FactionsEnum.Hell, UnitTypesEnum.Artillery), "Soul-Mortar" },
            { (FactionsEnum.Hell, UnitTypesEnum.Special), "Seraph of the Void" }
  };

    }
}
