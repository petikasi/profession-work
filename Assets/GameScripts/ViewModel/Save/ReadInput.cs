using System;
using Unity.VisualScripting;
using UnityEngine;

public class ReadInput : MonoBehaviour
{
    [SerializeField] private string inputText;
    public void ReadStringInput( string s) 
    {
        DeckBuilderController.Instance.DeckInBuilding.NAME = s;
        Debug.Log("The name is : " + DeckBuilderController.Instance.DeckInBuilding.NAME);
    }
}
