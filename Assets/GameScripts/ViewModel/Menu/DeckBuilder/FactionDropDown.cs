using System;
using UnityEngine;
using TMPro;
using Assets.GameScripts.Model.Game.GameControllerFolder;

namespace Assets.GameScripts.ViewModel.DeckBuilderMappa.DeckBuilder
{
    public class FactionDropDown : MonoBehaviour
    {
        [SerializeField] string[] UnitPanelPrefParent;
        [SerializeField] TMP_Dropdown dropdown;
        void Start()
        {
            MakeStringsForDropdown();
        }

        private void MakeStringsForDropdown()
        {


            foreach (FactionsEnum faction in Enum.GetValues(typeof(FactionsEnum)))
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(faction.ToString()));
            }
            dropdown.RefreshShownValue();
            Debug.Log("DropDown:OK");
        }

        public void GetDropDownValue()
        {
            FactionsEnum selectedFaction = (FactionsEnum)dropdown.value;
            Debug.Log(selectedFaction + " in view");
            DeckBuilderController.Instance.ChangeFaction(selectedFaction);



        }

        public void Refresh()
        {
            dropdown.RefreshShownValue();
        }
    }

}