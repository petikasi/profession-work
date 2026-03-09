using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class ScrollViewScript : MonoBehaviour
{
    [SerializeField] GameObject UnitPanelPref;
    [SerializeField] Transform UnitPanelParent;
    public  List<UnitSprite>  unitSprites;

    public void Awake()
    {
        unitSprites= new List<UnitSprite> ();
        AddSpritesToList();
    }

    private void OnEnable()
    {
        DeckBuilderController.Instance.OnFactionChanged += Reload;
        LoadUnitsView();
    }

    private void OnDisable()
    {
        DeckBuilderController.Instance.OnFactionChanged -= Reload;
        Clear();
    }


    private void LoadUnitsView()
    {
        foreach (Transform child in UnitPanelParent)
        {
            Destroy(child.gameObject);
        }
        foreach (UnitTypes unitType in Enum.GetValues(typeof(UnitTypes)))
        {
            string name = DeckBuilderController.Instance.DeckInBuilding.FactionsGet.ToString() + " "+ unitType.ToString();
            GameObject unitpanelGO = Instantiate(UnitPanelPref, UnitPanelParent);
            unitpanelGO.name = UnitPanelPref.name + " " + unitType.ToString();
            UnitPanelItem panelUI = unitpanelGO.GetComponent<UnitPanelItem>();
            UnitSprite sprite = unitSprites.Find(e => e.ByUnitAndFaction(DeckBuilderController.Instance.DeckInBuilding.FactionsGet, unitType));
            if (sprite != null) {
                
                panelUI.Initialize(
               name,
               sprite.GetSprite,
               () => DeckBuilderController.Instance.DeckInBuilding.GetCountUnit(unitType),
               () => DeckBuilderController.Instance.Add(unitType),
               () => DeckBuilderController.Instance.Remove(unitType)
            );
            }

        }
        Debug.Log("UnitPanels Loaded");
            




    }
    public void Clear() 
    {
        foreach (Transform child in UnitPanelParent)
        {
            Destroy(child.gameObject);
        }
    }
    public void Reload()
    {
 
        LoadUnitsView();
    }

    private void AddSpritesToList() 
    {

        foreach (Factions fac in Enum.GetValues(typeof( Factions))) 
        {
            
            foreach (UnitTypes unitType in Enum.GetValues(typeof(UnitTypes)))
            {
                string key = $"UnitPictures/{fac}/{unitType}"; ;
                Sprite sprite = Resources.Load<Sprite>(key);
                if (sprite == null) 
                { 

                }
                unitSprites.Add(
                    new UnitSprite(fac, unitType, sprite)
                );

            }

            Debug.Log($"{fac} finished loading into memory");
        }

    }





}
