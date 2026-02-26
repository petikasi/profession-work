using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using GameScripts.Model.Units;
[Serializable]
public class Deck
{

    private const int basealue= 100000;

    #region variables
    [SerializeField] private string id;
    [SerializeField] private Factions faction = Factions.Human;
    [SerializeField] private int Money = 100000;
    [SerializeField] private string name;
    [SerializeField] private List<UnitTypes> starterDeck = new();
    public string NAME
    {
        get => name;
        set => name = value;
    }
    public event Action<int> OnMoneyChanged;
    #endregion



    public int Count => starterDeck.Count;
    public string ID => id;

    public Factions FactionsGet => faction;    
    public List<UnitTypes> GetHoleListUnit() => starterDeck;
    public Deck (List<UnitTypes> units, Factions faction)
    {
        this.starterDeck = units;
        this.faction=faction;
        this.id = Guid.NewGuid().ToString();
    }
    public Deck()
    {
        this.starterDeck = new List<UnitTypes>();
        this.faction = Factions.Human;
        this.id = Guid.NewGuid().ToString();
    }

    public int GETMONEY => Money;
    public int GetPreisofUnit( UnitTypes unit) 
    {

        switch (unit) 
        {
            case UnitTypes.BasicMelee:
                return UnitPrices.PRICEOFBASICMELEE;;
            case UnitTypes.Ranged:
                return UnitPrices.PRICEOFRANGED;
            case UnitTypes.AdvancedMelee:
                return UnitPrices.PRICEOFADVANCEDMELEE;
            case UnitTypes.Wizard:
                return UnitPrices.PRICEOFWIZARD;
            case UnitTypes.Artillery:
                return UnitPrices.PRICEOFARTILLERY;
            case UnitTypes.Special:
                return UnitPrices.PRICEOFSECIAL;
        }

        return int.MaxValue;


    }
    public int GetCountUnit(UnitTypes unit)
    {
        int count = 0;

        foreach (UnitTypes u in starterDeck)
        {

            if (u == unit)
            {
                count++;
            }


        }
        return count;

    }
    public void ClearDeck() 
    {
        
        starterDeck = new List<UnitTypes>();
        NAME = null;
        Money = basealue;
    }

    #region Add&RemoveUnits

    public void Add( UnitTypes unit) 
    {

            Money -= GetPreisofUnit(unit);
            starterDeck.Add(unit);
 

        
    }
    public void Remove(UnitTypes unit)
    {

            starterDeck.Remove(unit);
            Money += GetPreisofUnit(unit);
            


    }

    #endregion

    #region Setters
    public List<UnitTypes> GetCountofUnits(Func<UnitTypes, bool> predicate)
    {
        List<UnitTypes> searchedunits = new();

        foreach (UnitTypes u in starterDeck)
        {

            foreach (var unit in starterDeck)
            {
                if (predicate(unit))
                    searchedunits.Add(unit);
            }


        }
        return searchedunits;

    }
    #endregion

    #region ChangeFaction
    public void ChangeFaction(Factions newFaction)
    {
        faction = newFaction;
        Debug.Log(newFaction+" in Model");
    }
    #endregion
}


