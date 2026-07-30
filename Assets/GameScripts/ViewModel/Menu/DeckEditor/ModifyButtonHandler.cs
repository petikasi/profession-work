using Assets.GameScripts.Model.Game.GameControllerFolder;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameScripts.ViewModel.DeckEditor
{
    public class ModifyButtonHandler : MonoBehaviour
    {
        [SerializeField] private Button ModifyButton;

        private void Start()
        {


            ModifyButton.onClick.RemoveAllListeners();
            ModifyButton.onClick.AddListener(()
                => {
                    Debug.Log("Modification started");
                    if (DeckLoaderController.Instance.SelectedDeck != null)
                    {
                        Debug.Log("Modification started");
                        DeckLoaderController.Instance.AddSelectedDeckToDeckBuilder();
                    }

                }
            );
        }

    }
}
