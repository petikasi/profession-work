using Assets.GameScripts.Model.Game.GameControllerFolder;
using Assets.GameScripts.ViewModel.CameraAndGrapichs.Grapic;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.GameScripts.ViewModel.Game.UnitSelectorCanvas
{



    public class UnitCountpanel : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image unitImage;
        [SerializeField] private TMP_Text unitCountText;

        private UnitTypesEnum myType;
        private FactionsEnum myFaction;
        private int currentCount;
        private bool zeroFromUnit = false;

        // EVENT: Jelezzük a Canvas-nak, ha változik a darabszám (true = csökkent, false = nőtt)
        public event Action<bool> OnUnitCountChanged;

        public void Initiate(UnitTypesEnum type, FactionsEnum faction, int currentCount)
        {
            myType = type;
            myFaction = faction;
            this.currentCount = currentCount;

            if (unitCountText != null)
            {
                unitCountText.text = currentCount.ToString();
            }

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

        public void OnPointerClick(PointerEventData eventData)
        {
            if (currentCount <= 0 || eventData.button != PointerEventData.InputButton.Left) return;

            if (UserGameController.Instance != null)
            {
                UserGameController.Instance.UnitandFactionSet(myType, myFaction, this);
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

            // Értesítjük a Canvast, hogy 1 egységet felraktunk a pályára!
            OnUnitCountChanged?.Invoke(true);
        }

        public void IncreaseCount()
        {
            currentCount++;

            if (unitCountText != null)
            {
                unitCountText.text = currentCount.ToString();
            }

            if (currentCount > 0 && zeroFromUnit)
            {
                if (unitImage != null)
                {
                    unitImage.color = Color.white;
                }
                zeroFromUnit = false;
            }

            // Értesítjük a Canvast, hogy 1 egységet visszavettünk a pályáról!
            OnUnitCountChanged?.Invoke(false);
        }
    }
}
