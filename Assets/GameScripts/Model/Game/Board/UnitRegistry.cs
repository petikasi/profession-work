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
        private Dictionary<(UnitTypesEnum,FactionsEnum ), UnitRegistryEntry> unitRegistry;

        public void InitializeRegistry(GameObject[] prefabs)
        {

            if (prefabs == null || prefabs.Length != 24)
            {
                Debug.LogError($"Hiba: A prefabs tömb mérete {prefabs?.Length ?? 0}, de 24-nek kellene lennie! Kérlek ellenőrizd az Inspectort!");
                return;
            }

            unitRegistry = new Dictionary<(UnitTypesEnum, FactionsEnum), UnitRegistryEntry>();
            // --- ELVEN ---
            unitRegistry.Add((UnitTypesEnum.BasicMelee, FactionsEnum.Elven), new UnitRegistryEntry(prefabs[0], typeof(ElvenMelee)));
            unitRegistry.Add((UnitTypesEnum.AdvancedMelee, FactionsEnum.Elven), new UnitRegistryEntry(prefabs[1], typeof(ElvenAdvancedMelee)));
            unitRegistry.Add((UnitTypesEnum.Ranged, FactionsEnum.Elven), new UnitRegistryEntry(prefabs[2], typeof(ElvenRanged)));
            unitRegistry.Add((UnitTypesEnum.Wizard, FactionsEnum.Elven), new UnitRegistryEntry(prefabs[3], typeof(ElvenWizard)));
            unitRegistry.Add((UnitTypesEnum.Artillery, FactionsEnum.Elven), new UnitRegistryEntry(prefabs[4], typeof(ElvenArtillery)));
            unitRegistry.Add((UnitTypesEnum.Special, FactionsEnum.Elven), new UnitRegistryEntry(prefabs[5], typeof(ElvenSpecial)));

            // --- HELL ---
            unitRegistry.Add((UnitTypesEnum.BasicMelee, FactionsEnum.Hell), new UnitRegistryEntry(prefabs[6], typeof(HellMelee)));
            unitRegistry.Add((UnitTypesEnum.AdvancedMelee, FactionsEnum.Hell), new UnitRegistryEntry(prefabs[7], typeof(HellAdvancedMelee)));
            unitRegistry.Add((UnitTypesEnum.Ranged, FactionsEnum.Hell), new UnitRegistryEntry(prefabs[8], typeof(HellRanged)));
            unitRegistry.Add((UnitTypesEnum.Wizard, FactionsEnum.Hell), new UnitRegistryEntry(prefabs[9], typeof(HellWizard)));
            unitRegistry.Add((UnitTypesEnum.Artillery, FactionsEnum.Hell), new UnitRegistryEntry(prefabs[10], typeof(HellArtillery)));
            unitRegistry.Add((UnitTypesEnum.Special, FactionsEnum.Hell), new UnitRegistryEntry(prefabs[11], typeof(HellSpecial)));

            // --- HUMAN ---
            unitRegistry.Add((UnitTypesEnum.BasicMelee, FactionsEnum.Human), new UnitRegistryEntry(prefabs[12], typeof(HumanMelee)));
            unitRegistry.Add((UnitTypesEnum.AdvancedMelee, FactionsEnum.Human), new UnitRegistryEntry(prefabs[13], typeof(HumanAdvancedMelee)));
            unitRegistry.Add((UnitTypesEnum.Ranged, FactionsEnum.Human), new UnitRegistryEntry(prefabs[14], typeof(HumanRanged)));
            unitRegistry.Add((UnitTypesEnum.Wizard, FactionsEnum.Human), new UnitRegistryEntry(prefabs[15], typeof(HumanWizard)));
            unitRegistry.Add((UnitTypesEnum.Artillery, FactionsEnum.Human), new UnitRegistryEntry(prefabs[16], typeof(HumanArtillery)));
            unitRegistry.Add((UnitTypesEnum.Special, FactionsEnum.Human), new UnitRegistryEntry(prefabs[17], typeof(HumanSpecial)));

            // --- ORK ---
            unitRegistry.Add((UnitTypesEnum.BasicMelee, FactionsEnum.Orks), new UnitRegistryEntry(prefabs[18], typeof(OrkMelee)));
            unitRegistry.Add((UnitTypesEnum.AdvancedMelee, FactionsEnum.Orks), new UnitRegistryEntry(prefabs[19], typeof(OrkAdvancedMelee)));
            unitRegistry.Add((UnitTypesEnum.Ranged, FactionsEnum.Orks), new UnitRegistryEntry(prefabs[20], typeof(OrkRanged)));
            unitRegistry.Add((UnitTypesEnum.Wizard, FactionsEnum.Orks), new UnitRegistryEntry(prefabs[21], typeof(OrkWizard)));
            unitRegistry.Add((UnitTypesEnum.Artillery, FactionsEnum.Orks), new UnitRegistryEntry(prefabs[22], typeof(OrkArtillery)));
            unitRegistry.Add((UnitTypesEnum.Special, FactionsEnum.Orks), new UnitRegistryEntry(prefabs[23], typeof(OrkSpecial)));


        }
        public UnitRegistryEntry GetEntry(UnitTypesEnum type, FactionsEnum faction)
        {
            return unitRegistry.TryGetValue((type, faction), out var entry) ? entry : null;
        }
    }
}
