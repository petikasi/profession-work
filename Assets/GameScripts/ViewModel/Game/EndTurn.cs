using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
