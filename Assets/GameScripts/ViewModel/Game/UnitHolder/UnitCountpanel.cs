using System;
using Assets.GameScripts.ViewModel.CameraAndGrapichs.Grapic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Assets.GameScripts.ViewModel.Game.UnitHolder
{
    public class UnitCountpanel : MonoBehaviour
    {
        [SerializeField] private Image unitImage;
        [SerializeField] private TMP_Text unitCountText;

        private UnitTypes myType;
        private Factions myFaction;

        /// <summary>
        /// Ezt a függvényt hívja meg a UnitCanvas, amikor létrehozza a panelt.
        /// </summary>
        public void Initiate(UnitTypes type, Factions faction, int currentCount)
        {
            myType = type;
            myFaction = faction;

            // 1. Beállítjuk a darabszámot a szövegmezőbe
            if (unitCountText != null)
            {
                unitCountText.text = currentCount.ToString();
            }

            // 2. Beállítjuk a képet (ikon) a frakció és típus alapján
            if (unitImage != null)
            {
                var match = PictureLoder.Instance.GETUNITPICTURES.FirstOrDefault(us => us.ByUnitAndFaction(myFaction, myType));
                Debug.Log(match);
                if (match != null)
                {
                    unitImage.sprite = match.GetSprite;
                }
                else
                {
                    Debug.LogError($"Nem található kép a következőhöz: {myFaction} - {myType}");
                }
            }
        }
    }
}
