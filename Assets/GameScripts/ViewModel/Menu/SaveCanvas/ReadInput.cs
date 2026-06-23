using Assets.GameScripts.Model.Game.GameControllerFolder;
using UnityEngine;

namespace Assets.GameScripts.ViewModel.DeckBuilderMappa.SaveCanvas
{
    public class ReadInput : MonoBehaviour
    {
        [SerializeField] private string inputText;
        public void ReadStringInput(string s)
        {
            DeckBuilderController.Instance.DeckInBuilding.NAME = s;
            Debug.Log("The name is : " + DeckBuilderController.Instance.DeckInBuilding.NAME);
        }
    }
}