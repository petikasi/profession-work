using TMPro;
using UnityEngine;
using Assets.GameScripts.Model.Game.GameControllerFolder;

namespace Assets.GameScripts.ViewModel.DeckBuilderMappa.DeckBuilder { 
    public class MoneyScript : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;


        private void OnEnable()
        {
            DeckBuilderController.Instance.OnMoneyChanged += UpdateText;
            UpdateText(DeckBuilderController.Instance.DeckInBuilding.GETMONEY);
        }

        private void OnDisable()
        {
            DeckBuilderController.Instance.OnMoneyChanged -= UpdateText;
        }
        private void UpdateText(int value)
        {
            moneyText.text = $"Money Remaining: {value}";
        }
    }
}
