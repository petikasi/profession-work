using Assets.GameScripts.Model.Game.GameControllerFolder;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameScripts.ViewModel.Game.UnitSelectorCanvas
{
    public class StartButton : MonoBehaviour
    {
        [SerializeField]private Button startButton;

        private void Awake()
        {
            // A UI gomb eseményét nyugodtan beállíthatjuk Awake-ben
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(() =>
            {
                if (UserGameController.Instance != null)
                {
                    UserGameController.Instance.OnEveryUnitPlaced -= SetStartButton;
                }
                UserGameController.Instance.SetPlayerToActive();
                Destroy(gameObject);
            });
        }

        private void OnEnable()
        {
            startButton.gameObject.SetActive(false);

            if (UserGameController.Instance != null)
            {
                UserGameController.Instance.OnEveryUnitPlaced += SetStartButton;
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
            }
        }

        private void SetStartButton(bool status)
        {
            Debug.Log($"[StartButton] SetStartButton meghívva, új állapot: {status}");
            startButton.gameObject.SetActive(status);
        }
    }
}