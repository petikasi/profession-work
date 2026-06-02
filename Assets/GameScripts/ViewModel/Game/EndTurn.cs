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
        }

        public void ActivateButton() 
        {
            EndTurnButton.gameObject.SetActive(true);
            isAtive = true;
        }


    }
}
