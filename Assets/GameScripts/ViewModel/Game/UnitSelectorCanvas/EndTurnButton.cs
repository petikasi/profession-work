using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameScripts.ViewModel.Game.UnitSelectorCanvas
{
    public class EndTurnButton : MonoBehaviour
    {
        [SerializeField] private Button endTurnButton;

        private void Awake()
        {
            endTurnButton.onClick.RemoveAllListeners();
            endTurnButton.onClick.AddListener(() =>
            {
                if (UserGameController.Instance != null)
                {
                    UserGameController.Instance.OnActivateEndTurnButton += SetEndButtonStatus;
                }

                UserGameController.Instance.EndPlayerTurn();
            });
        }

        private void OnEnable()
        {
            endTurnButton.gameObject.SetActive(false);

            if (UserGameController.Instance != null)
            {
                UserGameController.Instance.OnActivateEndTurnButton += SetEndButtonStatus;
            }
            else
            {
                Debug.LogError("[StartButton] UserGameController.Instance még mindig NULL!");
            }
        }

        private void OnDisable()
        {

            if (UserGameController.Instance != null)
            {
                UserGameController.Instance.OnActivateEndTurnButton -= SetEndButtonStatus;
            }
        }

        private void SetEndButtonStatus(bool status)
        {
            Debug.Log($"[Endbutton] Endbutton meghívva, új állapot: {status}");
            endTurnButton.gameObject.SetActive(status);
        }

    }
}