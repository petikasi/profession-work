using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameScripts.Persistence.Database
{
    [CreateAssetMenu(fileName = "UnitDatabase", menuName = "Game/Unit Database")]
    public class UnitDatabase : ScriptableObject
    {
        public List<UnitData> allUnits;

        public UnitData GetUnitData(UnitTypes type)
        {
            return allUnits.Find(u => u.unitType == type);
        }
    }
}
