using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.GameScripts.ViewModel.DeckEditor;
using UnityEngine;

namespace Assets.GameScripts.ViewModel.Graphic
{
    class PictureLoder :MonoBehaviour
    {
        public static PictureLoder Instance { get; private set; }
        private List<UnitSprite> unitSprites;
        private List<FactionSprites> factionSprites;
        public List<UnitSprite> GETUNITPICTURES => unitSprites;
        public List<FactionSprites> GETFACTIONPICTURES => factionSprites;



        public void Awake()
        {

            // Singleton logic
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // optional

            unitSprites = new List<UnitSprite>();
            factionSprites = new List<FactionSprites>();
            AddFactionSpritesToList();
            AddUnitSpritesToList();
        }

        private void AddFactionSpritesToList()
        {

            foreach (Factions fac in Enum.GetValues(typeof(Factions)))
            {

                string key = $"FactionPictures/{fac}";
                Sprite sprite = Resources.Load<Sprite>(key);
                if (sprite == null)
                {
                    Debug.LogWarning($"Sprite for {fac} not found at key: {key}");
                }
                factionSprites.Add(
                    new FactionSprites(fac, sprite)
                );

                Debug.Log($"{fac} faction finished loading into memory");
            }

        }

        private void AddUnitSpritesToList()
        {

            foreach (Factions fac in Enum.GetValues(typeof(Factions)))
            {

                foreach (UnitTypes unitType in Enum.GetValues(typeof(UnitTypes)))
                {
                    string key = $"UnitPictures/{fac}/{unitType}"; ;
                    Sprite sprite = Resources.Load<Sprite>(key);
                    if (sprite == null)
                    {

                    }
                    unitSprites.Add(
                        new UnitSprite(fac, unitType, sprite)
                    );

                }

                Debug.Log($"{fac} finished loading into memory");
            }

        }


    }
}
