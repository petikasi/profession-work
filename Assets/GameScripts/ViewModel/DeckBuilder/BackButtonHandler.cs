using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameScripts.ViewModel.DeckBuilder
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
