using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

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


        foreach (Factions faction in Enum.GetValues(typeof(Factions)))
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(faction.ToString()));
        }
        dropdown.RefreshShownValue();
        Debug.Log("DropDown:OK");
    }

    public void GetDropDownValue() 
    {
        Factions selectedFaction = (Factions)dropdown.value;
        Debug.Log(selectedFaction+" in view");
        DeckBuilderController.Instance.ChangeFaction(selectedFaction);


     
    }

    public void Refresh()
    {
        dropdown.RefreshShownValue();
    }
}
