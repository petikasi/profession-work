using System.Linq;
using Assets.GameScripts.ViewModel.CameraAndGrapichs.Grapic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

namespace Assets.GameScripts.ViewModel.Game.UnitHolder
{



    public class UnitCountpanel : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image unitImage;
        [SerializeField] private TMP_Text unitCountText;

        private UnitTypes myType;
        private Factions myFaction;
        private int currentCount;

        /// <summary>
        /// Ezt a függvényt hívja meg a UnitCanvas, amikor létrehozza a panelt.
        /// </summary>
        public void Initiate(UnitTypes type, Factions faction, int currentCount)
        {
            myType = type;
            myFaction = faction;
            this.currentCount = currentCount;

            // 1. Beállítjuk a darabszámot a szövegmezőbe
            if (unitCountText != null)
            {
                unitCountText.text = currentCount.ToString();
            }

            // 2. Beállítjuk a képet (ikon) a frakció és típus alapján
            if (unitImage != null)
            {
                var match = PictureLoder.Instance.GETUNITPICTURES.FirstOrDefault(us => us.ByUnitAndFaction(myFaction, myType));
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

        /// <summary>
        /// Ez automatikusan lefut, ha a játékos bárhova rákattint a panelen belül!
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            // Ha elfogyott az egység, vagy nem a bal egérgombbal kattintottak, nem csinálunk semmit
            if (currentCount <= 0 || eventData.button != PointerEventData.InputButton.Left) return;

            if (GameController.Instance != null)
            {
                // JAVÍTÁS: Közvetlen értékadás helyett a metódust hívjuk, 
                // és átadjuk saját magunkat is (this) a számláló csökkentéséhez!
                GameController.Instance.UnitandFactionSet(myType, myFaction, this);

                Debug.Log($"Panel kattintás érzékelve: {myFaction} - {myType}");
            }
        }

        public void DecreaseCount()
        {
            currentCount--;

            if (unitCountText != null)
            {
                unitCountText.text = currentCount.ToString();
            }

            // Ha az utolsó darab is elfogyott, elszürkítjük a panelt
            if (currentCount <= 0 && unitImage != null)
            {
                unitImage.color = new Color(0.3f, 0.3f, 0.3f, 0.5f);
            }
        }

        public void IncreaseCount()
        {
            currentCount++;

            if (unitCountText != null)
            {
                unitCountText.text = currentCount.ToString();
            }
        }

    }
}
