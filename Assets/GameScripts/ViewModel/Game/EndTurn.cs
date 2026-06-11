using Assets.GameScripts.Model.Game.GameControllerFolder;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameScripts.ViewModel.Game
{
    public class EndTurn:MonoBehaviour
    {

        [SerializeField] private Button EndTurnButton;
        private bool isAtive=false;

        public void Start()
        {
            EndTurnButton.onClick.RemoveAllListeners();
            EndTurnButton.onClick.AddListener(() 
            =>
                {

                    isAtive = !isAtive;
                }
            );
            GameController.Instance.OnEveryUnitPlaced += FirstClick;
            EndTurnButton.gameObject.SetActive(false);
        }

        public void ActivateButton() 
        {
            EndTurnButton.gameObject.SetActive(true);
            isAtive = true;
        }
        public void FirstClick()
        {
            Text buttonText = EndTurnButton.GetComponentInChildren<Text>();
            Debug.Log(buttonText);
            Debug.Log("End Turn!");
            if (buttonText != null)
            {
                buttonText.text = "End Turn!";
                Debug.Log("Baj");
            }
            EndTurnButton.gameObject.SetActive(true);
            isAtive = true;
        }


    }
}
