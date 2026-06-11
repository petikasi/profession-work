using System.Linq;
using Assets.GameScripts.ViewModel.CameraAndGrapichs.Grapic;
using Assets.GameScripts.Model.Game.GameControllerFolder;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Assets.GameScripts.ViewModel.Game.UnitHolder
{



    public class UnitCountpanel : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image unitImage;
        [SerializeField] private TMP_Text unitCountText;

        private UnitTypesEnum myType;
        private FactionsEnum myFaction;
        private int currentCount;
        private bool zeroFromUnit= false;

        /// <summary>
        /// Ezt a függvényt hívja meg a UnitCanvas, amikor létrehozza a panelt.
        /// </summary>
        public void Initiate(UnitTypesEnum type, FactionsEnum faction, int currentCount)
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

            if (currentCount <= 0 && !zeroFromUnit)
            {
                unitImage.color = new Color(0.7f, 0.7f, 0.7f, 1.0f);
                zeroFromUnit = true;
            }
        }

        public void IncreaseCount()
        {
            currentCount++;

            if (unitCountText != null)
            {
                unitCountText.text = currentCount.ToString();
            }
            // Ha eddig le volt merülve (true volt), de most már újra nagyobb mint 0 a darabszám
            if (currentCount > 0 && zeroFromUnit)
            {
                if (unitImage != null)
                {
                    // Visszaállítjuk az eredeti, élénk, nem átlátszó színt (RGB: 1,1,1, Alpha: 1)
                    unitImage.color = Color.white;
                }

                // Visszaállítjuk a flaget, hiszen már nincs 0-n a számláló
                zeroFromUnit = false;
            }
        }

    }
}
