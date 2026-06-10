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
          // 1. Pakli: Kiegyensúlyozott menetelő sereg (16 egység)
    public static readonly List<(FactionsEnum Faction, UnitTypesEnum UnitType)> HumanBalancedDeck = new()
    {
        // Elővéd (6x Sworn-Brother)
        (FactionsEnum.Human, UnitTypesEnum.BasicMelee), (FactionsEnum.Human, UnitTypesEnum.BasicMelee),
        (FactionsEnum.Human, UnitTypesEnum.BasicMelee), (FactionsEnum.Human, UnitTypesEnum.BasicMelee),
        (FactionsEnum.Human, UnitTypesEnum.BasicMelee), (FactionsEnum.Human, UnitTypesEnum.BasicMelee),
        // Íjászsor (5x Vigil-Archer)
        (FactionsEnum.Human, UnitTypesEnum.Ranged), (FactionsEnum.Human, UnitTypesEnum.Ranged),
        (FactionsEnum.Human, UnitTypesEnum.Ranged), (FactionsEnum.Human, UnitTypesEnum.Ranged),
        (FactionsEnum.Human, UnitTypesEnum.Ranged),
        // Lovagok és Elit (3x Justiciar)
        (FactionsEnum.Human, UnitTypesEnum.AdvancedMelee), (FactionsEnum.Human, UnitTypesEnum.AdvancedMelee),
        (FactionsEnum.Human, UnitTypesEnum.AdvancedMelee),
        // Támogatók (2x Fate Weaver)
        (FactionsEnum.Human, UnitTypesEnum.Wizard), (FactionsEnum.Human, UnitTypesEnum.Wizard)
    };

        // 2. Pakli: Ostromló és Elit sereg (15 egység)
        public static readonly List<(FactionsEnum Faction, UnitTypesEnum UnitType)> HumanSiegeDeck = new()
    {
        // Védvonal (5x Sworn-Brother)
        (FactionsEnum.Human, UnitTypesEnum.BasicMelee), (FactionsEnum.Human, UnitTypesEnum.BasicMelee),
        (FactionsEnum.Human, UnitTypesEnum.BasicMelee), (FactionsEnum.Human, UnitTypesEnum.BasicMelee),
        (FactionsEnum.Human, UnitTypesEnum.BasicMelee),
        // Hátvéd hátulról lövőknek (3x Vigil-Archer)
        (FactionsEnum.Human, UnitTypesEnum.Ranged), (FactionsEnum.Human, UnitTypesEnum.Ranged),
        (FactionsEnum.Human, UnitTypesEnum.Ranged),
        // Nehéz tüzérség (4x Cataclysm Cannon)
        (FactionsEnum.Human, UnitTypesEnum.Artillery), (FactionsEnum.Human, UnitTypesEnum.Artillery),
        (FactionsEnum.Human, UnitTypesEnum.Artillery), (FactionsEnum.Human, UnitTypesEnum.Artillery),
        // Parancsnoki elit és a Főnix (2x Justiciar, 1x Archangel)
        (FactionsEnum.Human, UnitTypesEnum.AdvancedMelee), (FactionsEnum.Human, UnitTypesEnum.AdvancedMelee),
        (FactionsEnum.Human, UnitTypesEnum.Special)
    };

        // ==========================================
        // ORKS (Orkok) - Átlagosan 18 egység (Horda jellegűbb)
        // ==========================================

        // 1. Pakli: Horda roham - "Waaagh!" (18 egység)
        public static readonly List<(FactionsEnum Faction, UnitTypesEnum UnitType)> OrksHordeDeck = new()
    {
        // A Horda magja (10x Da Boyz)
        (FactionsEnum.Orks, UnitTypesEnum.BasicMelee), (FactionsEnum.Orks, UnitTypesEnum.BasicMelee),
        (FactionsEnum.Orks, UnitTypesEnum.BasicMelee), (FactionsEnum.Orks, UnitTypesEnum.BasicMelee),
        (FactionsEnum.Orks, UnitTypesEnum.BasicMelee), (FactionsEnum.Orks, UnitTypesEnum.BasicMelee),
        (FactionsEnum.Orks, UnitTypesEnum.BasicMelee), (FactionsEnum.Orks, UnitTypesEnum.BasicMelee),
        (FactionsEnum.Orks, UnitTypesEnum.BasicMelee), (FactionsEnum.Orks, UnitTypesEnum.BasicMelee),
        // Nehézfiúk páncélban (6x Iron-Crusher)
        (FactionsEnum.Orks, UnitTypesEnum.AdvancedMelee), (FactionsEnum.Orks, UnitTypesEnum.AdvancedMelee),
        (FactionsEnum.Orks, UnitTypesEnum.AdvancedMelee), (FactionsEnum.Orks, UnitTypesEnum.AdvancedMelee),
        (FactionsEnum.Orks, UnitTypesEnum.AdvancedMelee), (FactionsEnum.Orks, UnitTypesEnum.AdvancedMelee),
        // Vezérek (2x The Bloodaxe)
        (FactionsEnum.Orks, UnitTypesEnum.Special), (FactionsEnum.Orks, UnitTypesEnum.Special)
    };

        // 2. Pakli: Támogató sámán / Lövész sereg (16 egység)
        public static readonly List<(FactionsEnum Faction, UnitTypesEnum UnitType)> OrksShamanDeck = new()
    {
        // Húsfal a sámánok elé (5x Da Boyz)
        (FactionsEnum.Orks, UnitTypesEnum.BasicMelee), (FactionsEnum.Orks, UnitTypesEnum.BasicMelee),
        (FactionsEnum.Orks, UnitTypesEnum.BasicMelee), (FactionsEnum.Orks, UnitTypesEnum.BasicMelee),
        (FactionsEnum.Orks, UnitTypesEnum.BasicMelee),
        // Köpködők (5x Spite-Flinger)
        (FactionsEnum.Orks, UnitTypesEnum.Ranged), (FactionsEnum.Orks, UnitTypesEnum.Ranged),
        (FactionsEnum.Orks, UnitTypesEnum.Ranged), (FactionsEnum.Orks, UnitTypesEnum.Ranged),
        (FactionsEnum.Orks, UnitTypesEnum.Ranged),
        // Sámán kör (4x Boneseer)
        (FactionsEnum.Orks, UnitTypesEnum.Wizard), (FactionsEnum.Orks, UnitTypesEnum.Wizard),
        (FactionsEnum.Orks, UnitTypesEnum.Wizard), (FactionsEnum.Orks, UnitTypesEnum.Wizard),
        // Katapultok (2x Skull Lobber)
        (FactionsEnum.Orks, UnitTypesEnum.Artillery), (FactionsEnum.Orks, UnitTypesEnum.Artillery)
    };

        // ==========================================
        // ELVES (Elfek) - Átlagosan 15 egység (Elit jellegűbb)
        // ==========================================

        // 1. Pakli: Mágikus és Íjász sereg (15 egység)
        public static readonly List<(FactionsEnum Faction, UnitTypesEnum UnitType)> ElvenRangedDeck = new()
    {
        // Őrség (4x Sentinel)
        (FactionsEnum.Elven, UnitTypesEnum.BasicMelee), (FactionsEnum.Elven, UnitTypesEnum.BasicMelee),
        (FactionsEnum.Elven, UnitTypesEnum.BasicMelee), (FactionsEnum.Elven, UnitTypesEnum.BasicMelee),
        // Mesteríjászok (6x Long-Watcher)
        (FactionsEnum.Elven, UnitTypesEnum.Ranged), (FactionsEnum.Elven, UnitTypesEnum.Ranged),
        (FactionsEnum.Elven, UnitTypesEnum.Ranged), (FactionsEnum.Elven, UnitTypesEnum.Ranged),
        (FactionsEnum.Elven, UnitTypesEnum.Ranged), (FactionsEnum.Elven, UnitTypesEnum.Ranged),
        // Bölcsek (3x Archon of Echoes)
        (FactionsEnum.Elven, UnitTypesEnum.Wizard), (FactionsEnum.Elven, UnitTypesEnum.Wizard),
        (FactionsEnum.Elven, UnitTypesEnum.Wizard),
        // Fantomok (2x The Pale Revenant)
        (FactionsEnum.Elven, UnitTypesEnum.Special), (FactionsEnum.Elven, UnitTypesEnum.Special)
    };

        // 2. Pakli: Ősi védelmezők (15 egység)
        public static readonly List<(FactionsEnum Faction, UnitTypesEnum UnitType)> ElvenAncientDeck = new()
    {
        // Alapvédelem (3x Sentinel)
        (FactionsEnum.Elven, UnitTypesEnum.BasicMelee), (FactionsEnum.Elven, UnitTypesEnum.BasicMelee),
        (FactionsEnum.Elven, UnitTypesEnum.BasicMelee),
        // Elit testőrség (6x Oathtaker)
        (FactionsEnum.Elven, UnitTypesEnum.AdvancedMelee), (FactionsEnum.Elven, UnitTypesEnum.AdvancedMelee),
        (FactionsEnum.Elven, UnitTypesEnum.AdvancedMelee), (FactionsEnum.Elven, UnitTypesEnum.AdvancedMelee),
        (FactionsEnum.Elven, UnitTypesEnum.AdvancedMelee), (FactionsEnum.Elven, UnitTypesEnum.AdvancedMelee),
        // Támogató mágia (3x Archon of Echoes)
        (FactionsEnum.Elven, UnitTypesEnum.Wizard), (FactionsEnum.Elven, UnitTypesEnum.Wizard),
        (FactionsEnum.Elven, UnitTypesEnum.Wizard),
        // Ősi balliszták (3x Gaze of the Ancients)
        (FactionsEnum.Elven, UnitTypesEnum.Artillery), (FactionsEnum.Elven, UnitTypesEnum.Artillery),
        (FactionsEnum.Elven, UnitTypesEnum.Artillery)
    };

        // ==========================================
        // HELL (Pokol) - Átlagosan 16 egység
        // ==========================================

        // 1. Pakli: Kárhozott rohamcsapat (16 egység)
        public static readonly List<(FactionsEnum Faction, UnitTypesEnum UnitType)> HellRushDeck = new()
    {
        // Karmos démonok (6x Dread-Claw)
        (FactionsEnum.Hell, UnitTypesEnum.BasicMelee), (FactionsEnum.Hell, UnitTypesEnum.BasicMelee),
        (FactionsEnum.Hell, UnitTypesEnum.BasicMelee), (FactionsEnum.Hell, UnitTypesEnum.BasicMelee),
        (FactionsEnum.Hell, UnitTypesEnum.BasicMelee), (FactionsEnum.Hell, UnitTypesEnum.BasicMelee),
        // Pokoli lovagok (7x Hell-Knight)
        (FactionsEnum.Hell, UnitTypesEnum.AdvancedMelee), (FactionsEnum.Hell, UnitTypesEnum.AdvancedMelee),
        (FactionsEnum.Hell, UnitTypesEnum.AdvancedMelee), (FactionsEnum.Hell, UnitTypesEnum.AdvancedMelee),
        (FactionsEnum.Hell, UnitTypesEnum.AdvancedMelee), (FactionsEnum.Hell, UnitTypesEnum.AdvancedMelee),
        (FactionsEnum.Hell, UnitTypesEnum.AdvancedMelee),
        // Főnökök (3x Seraph of the Void)
        (FactionsEnum.Hell, UnitTypesEnum.Special), (FactionsEnum.Hell, UnitTypesEnum.Special),
        (FactionsEnum.Hell, UnitTypesEnum.Special)
    };

        // 2. Pakli: Sötét rituálé / Tüzérségi kontroll (16 egység)
        public static readonly List<(FactionsEnum Faction, UnitTypesEnum UnitType)> HellControlDeck = new()
    {
        // Csapda elterelés (4x Dread-Claw)
        (FactionsEnum.Hell, UnitTypesEnum.BasicMelee), (FactionsEnum.Hell, UnitTypesEnum.BasicMelee),
        (FactionsEnum.Hell, UnitTypesEnum.BasicMelee), (FactionsEnum.Hell, UnitTypesEnum.BasicMelee),
        // Távoli lövészek (4x Void-Stalker)
        (FactionsEnum.Hell, UnitTypesEnum.Ranged), (FactionsEnum.Hell, UnitTypesEnum.Ranged),
        (FactionsEnum.Hell, UnitTypesEnum.Ranged), (FactionsEnum.Hell, UnitTypesEnum.Ranged),
        // Átokszórók (4x Calamity-Weaver)
        (FactionsEnum.Hell, UnitTypesEnum.Wizard), (FactionsEnum.Hell, UnitTypesEnum.Wizard),
        (FactionsEnum.Hell, UnitTypesEnum.Wizard), (FactionsEnum.Hell, UnitTypesEnum.Wizard),
        // Lélekmozsarak (4x Soul-Mortar)
        (FactionsEnum.Hell, UnitTypesEnum.Artillery), (FactionsEnum.Hell, UnitTypesEnum.Artillery),
        (FactionsEnum.Hell, UnitTypesEnum.Artillery), (FactionsEnum.Hell, UnitTypesEnum.Artillery)
    };

    }
}
