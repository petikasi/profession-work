using Assets.GameScripts.Model.Game.GameControllerFolder;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GameScripts.ViewModel.DeckBuilderMappa.SaveCanvas {
    public class SaveDeckButton : MonoBehaviour
    {

        [SerializeField] private Button saveButtonObj;

        private void Start()
        {

            saveButtonObj.onClick.RemoveAllListeners();
            saveButtonObj.onClick.AddListener(()
                =>
            {
                Debug.Log(DeckBuilderController.Instance.DeckInBuilding.NAME);
                if (DeckBuilderController.Instance.DeckInBuilding.NAME != null)
                {
                    Debug.Log("Saving started");
                    DeckBuilderController.Instance.InitializeSave();
                }

            }
            );
        }


    }
}