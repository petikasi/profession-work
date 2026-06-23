using UnityEngine;
using UnityEngine.UI;
using Assets.GameScripts.Model.Game.GameControllerFolder;

namespace Assets.GameScripts.ViewModel.DeckBuilderMappa.DeckBuilder
{
    public class BackButtonHandler : MonoBehaviour
    {

        [SerializeField] private Button backButton;

        private void Start()
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(()
                =>
            {
                Debug.Log("Back from Deckbuilder to deckmanager");
                DeckBuilderController.Instance.SetDeckToEmpty();

            }
            );
        }

    }
}
