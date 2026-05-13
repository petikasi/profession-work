using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameScripts.Model.Game.Board
{
    public class UnitRegistryEntry
    {
        public GameObject Prefab;
        public Type ScriptType;
        

        public UnitRegistryEntry(GameObject prefab, Type scriptType)
        {
            Prefab = prefab;
            ScriptType = scriptType;
         }
        
    }
}
