using System;
using System.Collections.Generic;
using UnityEngine;
using GameScripts.Model.Units;
using System.Linq;
[Serializable]
public class Deck
{

    private const int basealue= 100000;

    #region variables
    [SerializeField] private string id;
    [SerializeField] private FactionsEnum faction = FactionsEnum.Human;
    [SerializeField] private int Money = 100000;
    [SerializeField] private string name;
    [SerializeField] private List<UnitTypesEnum> starterDeck = new();

    public string NAME
    {
        get => name;
        set => name = value;

    }
    #endregion



    public int Count => starterDeck.Count;
    public string ID => id;

    public FactionsEnum FactionsGet => faction;    
    public List<UnitTypesEnum> GetHoleListUnit() => starterDeck;
    public Deck (List<UnitTypesEnum> units, FactionsEnum faction)
    {
        this.starterDeck = units;
        this.faction=faction;
        this.id = Guid.NewGuid().ToString();
        name = "";
    }
    public Deck()
    {
        this.starterDeck = new List<UnitTypesEnum>();
        this.faction = FactionsEnum.Human;
        this.id = Guid.NewGuid().ToString();
        name = "";
    }

    public int GETMONEY => Money;
    public int GetPreisofUnit( UnitTypesEnum unit) 
    {

        switch (unit) 
        {
            case UnitTypesEnum.BasicMelee:
                return UnitPrices.PRICEOFBASICMELEE;;
            case UnitTypesEnum.Ranged:
                return UnitPrices.PRICEOFRANGED;
            case UnitTypesEnum.AdvancedMelee:
                return UnitPrices.PRICEOFADVANCEDMELEE;
            case UnitTypesEnum.Wizard:
                return UnitPrices.PRICEOFWIZARD;
            case UnitTypesEnum.Artillery:
                return UnitPrices.PRICEOFARTILLERY;
            case UnitTypesEnum.Special:
                return UnitPrices.PRICEOFSECIAL;
        }

        return int.MaxValue;


    }
    public int GetCountUnit(UnitTypesEnum unit)
    {
        int count = 0;

        foreach (UnitTypesEnum u in starterDeck)
        {

            if (u == unit)
            {
                count++;
            }


        }
        return count;

    }

    #region Add&RemoveUnits

    public void Add( UnitTypesEnum unit) 
    {

            Money -= GetPreisofUnit(unit);
            starterDeck.Add(unit);
 

        
    }
    public void Remove(UnitTypesEnum unit)
    {

            starterDeck.Remove(unit);
            Money += GetPreisofUnit(unit);
            


    }
    #endregion

    #region Setters
    public int  GetCountofUnits(UnitTypesEnum predicate)
    {
       return  starterDeck.Where(unit => unit == predicate).ToList().Count;
    }
    #endregion

    #region ChangeFaction
    public void ChangeFaction(FactionsEnum newFaction)
    {
        faction = newFaction;
        Debug.Log(newFaction+" in Model");
    }
    #endregion
}


