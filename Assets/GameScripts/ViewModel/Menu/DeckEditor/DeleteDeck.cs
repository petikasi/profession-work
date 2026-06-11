using Assets.GameScripts.Model.Game.GameControllerFolder;
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
