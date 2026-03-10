using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GameScripts.Model.Deckmaker.Deks;
using Assets.GameScripts.Model.Game.GameController;
using GameScripts.Model.Units;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameScripts.ViewModel.DeckEditor
{

    public class DeleteDeck :MonoBehaviour
    {
        [SerializeField] Button deleteButton;

        private void OnEnable()
        { 
            deleteButton.onClick.AddListener(InitializeDeletingDeck);
        }

        private void OnDisable()
        {
            deleteButton.onClick.RemoveAllListeners();
        }

        public void InitializeDeletingDeck() 
        {

            if (DeckManagerController.Instance.SelectedDeck != null) 
            {
                DeckManagerController.Instance.RemoveSelectedDeck();
            }
        
        
        } 
    }
}
