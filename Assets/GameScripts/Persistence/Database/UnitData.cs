using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameScripts.Persistence.Database
{    

    [CreateAssetMenu(fileName = "NewUnitData", menuName = "Game/Unit Data")]
    public class UnitData : ScriptableObject
    {
        public UnitTypes unitType;       // Az Enum azonosító
        public GameObject unitPrefab;    // A 3D modell/Prefab
        public string scriptClassName;   // A C# osztály neve (pl. "HumanMelee")
    }
}
