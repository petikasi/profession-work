using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GameScripts.Model.Game.Players.BasicEnemy
{
    public class BasicEnemyBehaviour
    {
        /// <summary>
        /// Kiválaszt egy célpontot az ellenséges egységek közül a rácskordináták (TileX, TileZ) alapján súlyozva.
        /// A közelebbi egységek nagyobb eséllyel lesznek kiválasztva.
        /// </summary>
        /// <param name="selfUnit">A támadó bot egysége</param>
        /// <param name="possibleTargets">A lehetséges célpont egységek listája</param>
        public BaseUnit ChooseTargetBasedOnDistance(BaseUnit selfUnit, List<BaseUnit> possibleTargets)
        {
            if (selfUnit == null || possibleTargets == null || possibleTargets.Count == 0)
                return null;

            if (possibleTargets.Count == 1)
                return possibleTargets[0];

            List<float> weights = new List<float>();
            float totalWeight = 0f;

            // 1. Súlyok kiszámítása a TileX és TileZ koordináták alapján
            foreach (var target in possibleTargets)
            {
                if (target == null) continue;

                // Manhattan-távolság kiszámítása a rácson (|x1 - x2| + |z1 - z2|)
                int distance = Math.Abs(selfUnit.TileX - target.TileX) + Math.Abs(selfUnit.TileZ - target.TileZ);

                // Súly kiszámítása (minél kisebb a távolság, annál nagyobb a súly)
                float weight = 1f / distance;

                weights.Add(weight);
                totalWeight += weight;
            }

            // 2. Súlyozott véletlen sorsolás
            float randomValue = UnityEngine.Random.Range(0f, totalWeight);
            float currentSum = 0f;

            for (int i = 0; i < possibleTargets.Count; i++)
            {
                currentSum += weights[i];
                if (randomValue <= currentSum)
                {
                    return possibleTargets[i];
                }
            }

            return possibleTargets.LastOrDefault();
        }

    }
}
