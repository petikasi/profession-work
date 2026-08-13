using Assets.GameScripts.Model.Game.GameControllerFolder;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameScripts.ViewModel.Game.UnitSelectorCanvas
{
    public class StartButton : MonoBehaviour
    {
        [SerializeField]private Button startButton;

        private void Start()
        {
            startButton.gameObject.SetActive(false);

            UserGameController.Instance.OnEveryUnitPlaced += SetStartButton;

            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(() =>
            {
                Debug.Log("Deck Setted to active");
                if (UserGameController.Instance != null)
                {
                    UserGameController.Instance.OnEveryUnitPlaced -= SetStartButton;
                }

                UserGameController.Instance.SetPlayerToActive();
                DestroyButton();
            });
        }

        private void SetStartButton(bool stasus)
        {
            if (stasus == startButton.gameObject.activeSelf)
            {
                startButton.gameObject.SetActive(stasus);
            }
        }


        private void DestroyButton()
        {
            UserGameController.Instance.OnEveryUnitPlaced -= SetStartButton;
            Destroy(gameObject);
        }
    }
}