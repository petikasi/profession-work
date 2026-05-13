using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameScripts.Model.Game.Board
{
    public class UnitRegistry
    {
        private Dictionary<(UnitTypes,Factions ), UnitRegistryEntry> unitRegistry;

        public void InitializeRegistry(GameObject[] prefabs)
        {

            if (prefabs == null || prefabs.Length != 24)
            {
                Debug.LogError($"Hiba: A prefabs tömb mérete {prefabs?.Length ?? 0}, de 24-nek kellene lennie! Kérlek ellenőrizd az Inspectort!");
                return;
            }

            unitRegistry = new Dictionary<(UnitTypes, Factions), UnitRegistryEntry>();
            // --- ELVEN ---
            unitRegistry.Add((UnitTypes.BasicMelee, Factions.Elven), new UnitRegistryEntry(prefabs[0], typeof(ElvenMelee)));
            unitRegistry.Add((UnitTypes.AdvancedMelee, Factions.Elven), new UnitRegistryEntry(prefabs[1], typeof(ElvenAdvancedMelee)));
            unitRegistry.Add((UnitTypes.Ranged, Factions.Elven), new UnitRegistryEntry(prefabs[2], typeof(ElvenRanged)));
            unitRegistry.Add((UnitTypes.Wizard, Factions.Elven), new UnitRegistryEntry(prefabs[3], typeof(ElvenWizard)));
            unitRegistry.Add((UnitTypes.Artillery, Factions.Elven), new UnitRegistryEntry(prefabs[4], typeof(ElvenArtillery)));
            unitRegistry.Add((UnitTypes.Special, Factions.Elven), new UnitRegistryEntry(prefabs[5], typeof(ElvenSpecial)));

            // --- HELL ---
            unitRegistry.Add((UnitTypes.BasicMelee, Factions.Hell), new UnitRegistryEntry(prefabs[6], typeof(HellMelee)));
            unitRegistry.Add((UnitTypes.AdvancedMelee, Factions.Hell), new UnitRegistryEntry(prefabs[7], typeof(HellAdvancedMelee)));
            unitRegistry.Add((UnitTypes.Ranged, Factions.Hell), new UnitRegistryEntry(prefabs[8], typeof(HellRanged)));
            unitRegistry.Add((UnitTypes.Wizard, Factions.Hell), new UnitRegistryEntry(prefabs[9], typeof(HellWizard)));
            unitRegistry.Add((UnitTypes.Artillery, Factions.Hell), new UnitRegistryEntry(prefabs[10], typeof(HellArtillery)));
            unitRegistry.Add((UnitTypes.Special, Factions.Hell), new UnitRegistryEntry(prefabs[11], typeof(HellSpecial)));

            // --- HUMAN ---
            unitRegistry.Add((UnitTypes.BasicMelee, Factions.Human), new UnitRegistryEntry(prefabs[12], typeof(HumanMelee)));
            unitRegistry.Add((UnitTypes.AdvancedMelee, Factions.Human), new UnitRegistryEntry(prefabs[13], typeof(HumanAdvancedMelee)));
            unitRegistry.Add((UnitTypes.Ranged, Factions.Human), new UnitRegistryEntry(prefabs[14], typeof(HumanRanged)));
            unitRegistry.Add((UnitTypes.Wizard, Factions.Human), new UnitRegistryEntry(prefabs[15], typeof(HumanWizard)));
            unitRegistry.Add((UnitTypes.Artillery, Factions.Human), new UnitRegistryEntry(prefabs[16], typeof(HumanArtillery)));
            unitRegistry.Add((UnitTypes.Special, Factions.Human), new UnitRegistryEntry(prefabs[17], typeof(HumanSpecial)));

            // --- ORK ---
            unitRegistry.Add((UnitTypes.BasicMelee, Factions.Orks), new UnitRegistryEntry(prefabs[18], typeof(OrkMelee)));
            unitRegistry.Add((UnitTypes.AdvancedMelee, Factions.Orks), new UnitRegistryEntry(prefabs[19], typeof(OrkAdvancedMelee)));
            unitRegistry.Add((UnitTypes.Ranged, Factions.Orks), new UnitRegistryEntry(prefabs[20], typeof(OrkRanged)));
            unitRegistry.Add((UnitTypes.Wizard, Factions.Orks), new UnitRegistryEntry(prefabs[21], typeof(OrkWizard)));
            unitRegistry.Add((UnitTypes.Artillery, Factions.Orks), new UnitRegistryEntry(prefabs[22], typeof(OrkArtillery)));
            unitRegistry.Add((UnitTypes.Special, Factions.Orks), new UnitRegistryEntry(prefabs[23], typeof(OrkSpecial)));


        }
        public UnitRegistryEntry GetEntry(UnitTypes type, Factions faction)
        {
            return unitRegistry.TryGetValue((type, faction), out var entry) ? entry : null;
        }
    }
}
