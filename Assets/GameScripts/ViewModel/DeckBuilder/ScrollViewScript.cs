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
    }

    private void OnDisable()
    {
        DeckBuilderController.Instance.OnFactionChanged -= Reload;
    }

    public void Start()
    {
        LoadUnitsView();
    }

    private void LoadUnitsView()
    {
        foreach (UnitTypes unitType in Enum.GetValues(typeof(UnitTypes)))
        {
            string name = DeckBuilderController.Instance.CurrentDeck.FactionsGet.ToString() + " "+ unitType.ToString();
            GameObject unitpanelGO = Instantiate(UnitPanelPref, UnitPanelParent);
            unitpanelGO.name = UnitPanelPref.name + " " + unitType.ToString();
            UnitPanelItem panelUI = unitpanelGO.GetComponent<UnitPanelItem>();
            UnitSprite sprite = unitSprites.Find(e => e.ByUnitAndFaction(DeckBuilderController.Instance.CurrentDeck.FactionsGet, unitType));
            if (sprite != null) {
                
                panelUI.Initialize(
               name,
               sprite.GetSprite,
               () => DeckBuilderController.Instance.CurrentDeck.GetCountUnit(unitType),
               () => DeckBuilderController.Instance.Add(unitType),
               () => DeckBuilderController.Instance.Remove(unitType)
            );
            }

        }
        Debug.Log("UnitPanels Loaded");
            




    }
    private void Reload(Factions faction)
    {
        DeckBuilderController.Instance.CurrentDeck.ClearDeck();
        Clear();
        LoadUnitsView();
    }

    private void Clear()
    {
        foreach (Transform child in UnitPanelParent)
        {
            Destroy(child.gameObject);
        }
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
