using Assets.GameScripts.Model.Game.GameControllerFolder;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameScripts.ViewModel.Game.UnitSelectorCanvas
{
    public class StartButton : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        private void Awake()
        {
            if (startButton == null)
            {
                startButton = GetComponent<Button>();
            }

            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        private void OnEnable()
        {
            if (startButton != null)
            {
                startButton.gameObject.SetActive(false);
            }

            if (UserGameController.Instance != null)
            {
                UserGameController.Instance.OnEveryUnitPlaced += SetStartButton;
                UserGameController.Instance.OnDestroyUnececeryView += OnDestroyButton;
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
                UserGameController.Instance.OnEveryUnitPlaced -= SetStartButton;
                UserGameController.Instance.OnDestroyUnececeryView -= OnDestroyButton;
            }
        }

        private void OnStartButtonClicked()
        {
            if (UserGameController.Instance != null)
            {
                UserGameController.Instance.SetPlayerToActive();
            }
        }

        private void SetStartButton(bool status)
        {
            Debug.Log($"[StartButton] SetStartButton meghívva, új állapot: {status}");
            if (startButton != null)
            {
                startButton.gameObject.SetActive(status);
            }
        }

        private void OnDestroyButton()
        {
            Debug.Log("[StartButton] StartButton eltüntetése (Gameobject törlése)...");

            // Biztonságosan csak a gomb objektumát töröljük, nem az egész Canvast!
            if (startButton != null)
            {
                Destroy(startButton.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}